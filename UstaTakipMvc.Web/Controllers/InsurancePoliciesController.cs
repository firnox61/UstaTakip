using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using UstaTakipMvc.Web.Models.InsurancePolicies;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Controllers
{
    [Authorize]
    public class InsurancePoliciesController : Controller
    {
        private readonly HttpClient _api;

        // API genelde camelCase + case-insensitive
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public InsurancePoliciesController(IHttpClientFactory factory)
        {
            // Program.cs: services.AddHttpClient("UstaApi", c => c.BaseAddress = new Uri("http://localhost:5280/api/"));
            _api = factory.CreateClient("UstaApi");
        }

        // LIST
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _api.GetAsync("insurancepolicies", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<InsurancePolicyListDto>());
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<InsurancePolicyListDto>>>(_json, ct);
            var list = (wrap?.Success == true ? (wrap.Data ?? new()) : new());

            if (wrap?.Success == false && !string.IsNullOrWhiteSpace(wrap.Message))
                TempData["Error"] = wrap.Message;

            await LoadVehicles(ct); // ViewBag.VehicleMap (Guid -> Plate)
            return View(list);
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadVehicles(ct);
            ViewBag.Companies = InsuranceCompanies.All;
            return View(new InsurancePolicyCreateDto
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsurancePolicyCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            var resp = await _api.PostAsJsonAsync("insurancepolicies", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct); // IResult -> ApiDataResult<object>

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            TempData["Success"] = ok.message ?? "Poliçe başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"insurancepolicies/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            // 1) Tercihen doğrudan UpdateDto dönüyorsa:
            var wrapUpd = await resp.Content.ReadFromJsonAsync<ApiDataResult<InsurancePolicyUpdateDto>>(_json, ct);
            InsurancePolicyUpdateDto? model = wrapUpd?.Data;

            // 2) Bazı API'lerde ListDto döner; o zaman map'leyelim
            if (model is null)
            {
                var wrapList = await resp.Content.ReadFromJsonAsync<ApiDataResult<InsurancePolicyListDto>>(_json, ct);
                if (wrapList?.Success != true || wrapList.Data is null)
                {
                    TempData["Error"] = wrapList?.Message ?? wrapUpd?.Message ?? "Poliçe bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var d = wrapList.Data;
                model = new InsurancePolicyUpdateDto
                {
                    Id = d.Id,
                    CompanyName = d.CompanyName,
                    PolicyNumber = d.PolicyNumber,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    CoverageAmount = d.CoverageAmount,
                    VehicleId = d.VehicleId
                };
            }

            await LoadVehicles(ct, model.VehicleId);
            ViewBag.Companies = InsuranceCompanies.All;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InsurancePolicyUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            // API'niz PUT'ta route id İSTEMİYOR -> sadece body
            var resp = await _api.PutAsJsonAsync("insurancepolicies", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct); // IResult -> ApiDataResult<object>

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            TempData["Success"] = ok.message ?? "Poliçe güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"insurancepolicies/{id}", ct);
            var ok = await ReadResultEnvelope(resp, ct); // IResult -> ApiDataResult<object>

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = ok.message ?? "Poliçe silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ----------------- helpers -----------------

        /// Araç seçeneklerini (Id, Plate) ve VehicleId->Plate haritasını ViewBag'e yükler.
        private async Task LoadVehicles(CancellationToken ct, Guid? selected = null)
        {
            var resp = await _api.GetAsync("vehicles/list", ct);
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Vehicles = new List<VehicleOption>();
                ViewBag.VehicleSelected = selected;
                ViewBag.VehicleMap = new Dictionary<Guid, string>();
                TempData["Error"] = "Araç listesi alınamadı: " + await ReadApiErrorAsync(resp, ct);
                return;
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleListDto>>>(_json, ct);
            var vehicles = (wrapper?.Success == true ? (wrapper.Data ?? new()) : new());

            var opts = vehicles.Select(v => new VehicleOption
            {
                Id = v.Id,
                Plate = v.Plate ?? ""
            }).ToList();

            ViewBag.Vehicles = opts;
            ViewBag.VehicleSelected = selected;
            ViewBag.VehicleMap = opts.ToDictionary(v => v.Id, v => v.Plate);
        }

        /// API hata gövdesini okur. ProblemDetails varsa onu; yoksa IResult sarmalını; o da yoksa raw döndürür.
        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            // 1) ProblemDetails dene
            try
            {
                var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
                if (problem != null)
                {
                    var title = string.IsNullOrWhiteSpace(problem.Title) ? "Hata" : problem.Title;
                    var detail = string.IsNullOrWhiteSpace(problem.Detail) ? "" : " - " + problem.Detail;
                    return $"API {(int)resp.StatusCode} {title}{detail}";
                }
            }
            catch { /* yut */ }

            // 2) ApiDataResult<object> dene (IResult/IDataResult)
            try
            {
                var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<object>>(_json, ct);
                if (wrap != null && !string.IsNullOrWhiteSpace(wrap.Message))
                    return $"API {(int)resp.StatusCode}: {wrap.Message}";
            }
            catch { /* yut */ }

            // 3) raw
            var raw = await resp.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw)
                ? $"API {(int)resp.StatusCode} hatası (boş içerik)."
                : raw;
        }

        /// IResult/IDataResult (success, message) okumak için ortak yardımcı
        private static async Task<(bool success, string? message)> ReadResultEnvelope(HttpResponseMessage resp, CancellationToken ct)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            try
            {
                var wrap = JsonSerializer.Deserialize<ApiDataResult<object>>(body, _json);
                if (wrap != null)
                    return (wrap.Success, wrap.Message);
            }
            catch { /* yut */ }

            // JSON değilse / farklı şema ise status code'a göre karar
            return (resp.IsSuccessStatusCode, null);
        }

        // View için minimal araç DTO'su
        public class VehicleOption
        {
            public Guid Id { get; set; }
            public string Plate { get; set; } = "";
        }
    }
}
