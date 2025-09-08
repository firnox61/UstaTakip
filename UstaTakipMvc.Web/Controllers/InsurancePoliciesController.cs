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

        // camelCase + case-insensitive JSON profili (API genelde camelCase döner)
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

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<InsurancePolicyListDto>>>(_json, ct);
            var list = wrapper?.Data ?? new();

            // Index'te VehicleId -> Plate gösterebilmek için harita
            await LoadVehicles(ct); // ViewBag.VehicleMap (Guid -> string Plate)
            return View(list);
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadVehicles(ct); // ViewBag.Vehicles (Id, Plate) + VehicleMap + VehicleSelected
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
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            TempData["Success"] = "Poliçe başarıyla eklendi.";
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

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<InsurancePolicyUpdateDto>>(_json, ct);
            var item = wrapper?.Data;
            if (item is null)
            {
                TempData["Error"] = wrapper?.Message ?? "Poliçe bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            await LoadVehicles(ct, item.VehicleId);
            return View(item);
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

            var resp = await _api.PutAsJsonAsync($"insurancepolicies/{dto.Id}", dto, _json, ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicles(ct, dto.VehicleId);
                return View(dto);
            }

            TempData["Success"] = "Poliçe güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"insurancepolicies/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Poliçe silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ----------------- helpers -----------------

        /// <summary>
        /// Araç seçeneklerini (Id, Plate) ve VehicleId->Plate haritasını ViewBag'e yükler.
        /// ViewBag.Vehicles: List<VehicleOption>  (Id, Plate)
        /// ViewBag.VehicleMap: Dictionary<Guid, string>  (VehicleId -> Plate)
        /// ViewBag.VehicleSelected: Guid?  (drop-down default seçimi)
        /// </summary>
        private async Task LoadVehicles(CancellationToken ct, Guid? selected = null)
        {
            // Plate bilgisini garanti eden bir endpoint kullanıyoruz
            // Tercihen Vehicles/list -> VehicleListDto (Id, Plate, Brand, Model, CustomerName ...)
            // Yoksa Vehicles/options da olur; ama Plate döndüğünden emin ol.
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
            var vehicles = wrapper?.Data ?? new();

            // View'ların kullandığı tip: VehicleOption (Id, Plate)
            var opts = vehicles.Select(v => new VehicleOption
            {
                Id = v.Id,
                Plate = v.Plate ?? ""
            }).ToList();

            ViewBag.Vehicles = opts;
            ViewBag.VehicleSelected = selected;
            ViewBag.VehicleMap = opts.ToDictionary(v => v.Id, v => v.Plate);
        }

        /// <summary>
        /// API hata gövdesini okur. ProblemDetails varsa onu, yoksa ham metni döndürür.
        /// </summary>
        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
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
            catch { /* ProblemDetails değilse yut, raw oku */ }

            var raw = await resp.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw)
                ? $"API {(int)resp.StatusCode} hatası (boş içerik)."
                : raw;
        }

        // View için minimal araç DTO'su
        public class VehicleOption
        {
            public Guid Id { get; set; }
            public string Plate { get; set; } = "";
        }
    }
}
