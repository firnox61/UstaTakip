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

        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public InsurancePoliciesController(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("UstaApi");
        }

        // LIST -------------------------------------------------------
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
            var list = wrap?.Success == true ? wrap.Data ?? new() : new();

            await LoadVehicles(ct);
            return View(list);
        }

        // CREATE -----------------------------------------------------
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
                ViewBag.Companies = InsuranceCompanies.All;
                return View(dto);
            }

            var resp = await _api.PostAsJsonAsync("insurancepolicies", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                ViewBag.Companies = InsuranceCompanies.All;
                return View(dto);
            }

            TempData["Success"] = ok.message ?? "Poliçe başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT -------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"insurancepolicies/{id}", ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            // API doğrudan UpdateDto döndürüyorsa:
            var wrapUpd = await resp.Content.ReadFromJsonAsync<ApiDataResult<InsurancePolicyUpdateDto>>(_json, ct);
            InsurancePolicyUpdateDto? model = wrapUpd?.Data;

            // Eğer API ListDto döndürdüyse → UpdateDto'ya dönüştür
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
                    AgencyCode = d.AgencyCode,
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
                ViewBag.Companies = InsuranceCompanies.All;
                return View(dto);
            }

            var resp = await _api.PutAsJsonAsync("insurancepolicies", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                ViewBag.Companies = InsuranceCompanies.All;
                return View(dto);
            }

            TempData["Success"] = ok.message ?? "Poliçe güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE -----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"insurancepolicies/{id}", ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message ?? await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = ok.message ?? "Poliçe silindi.";
            return RedirectToAction(nameof(Index));
        }

        // HELPERS ----------------------------------------------------

        private async Task LoadVehicles(CancellationToken ct, Guid? selected = null)
        {
            var resp = await _api.GetAsync("vehicles/list", ct);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Vehicles = new List<VehicleOption>();
                ViewBag.VehicleSelected = selected;
                ViewBag.VehicleMap = new Dictionary<Guid, string>();
                return;
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleListDto>>>(_json, ct);
            var vehicles = wrapper?.Success == true ? wrapper.Data ?? new() : new();

            var opts = vehicles.Select(v => new VehicleOption
            {
                Id = v.Id,
                Plate = v.Plate ?? ""
            }).ToList();

            ViewBag.Vehicles = opts;
            ViewBag.VehicleSelected = selected;
            ViewBag.VehicleMap = opts.ToDictionary(v => v.Id, v => v.Plate);
        }

        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
                if (problem != null)
                {
                    var title = string.IsNullOrWhiteSpace(problem.Title) ? "Hata" : problem.Title;
                    var detail = string.IsNullOrWhiteSpace(problem.Detail) ? "" : $" - {problem.Detail}";
                    return $"API {(int)resp.StatusCode} {title}{detail}";
                }
            }
            catch { }

            try
            {
                var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<object>>(_json, ct);
                if (wrap != null && !string.IsNullOrWhiteSpace(wrap.Message))
                    return wrap.Message;
            }
            catch { }

            return await resp.Content.ReadAsStringAsync(ct);
        }

        private static async Task<(bool success, string? message)> ReadResultEnvelope(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                var wrap = JsonSerializer.Deserialize<ApiDataResult<object>>(await resp.Content.ReadAsStringAsync(ct), _json);
                if (wrap != null)
                    return (wrap.Success, wrap.Message);
            }
            catch { }

            return (resp.IsSuccessStatusCode, null);
        }

        public class VehicleOption
        {
            public Guid Id { get; set; }
            public string Plate { get; set; } = "";
        }
    }
}
