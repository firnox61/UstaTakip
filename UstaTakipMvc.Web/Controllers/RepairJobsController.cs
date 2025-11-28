using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using UstaTakipMvc.Web.Models.RepairJobs;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Controllers
{
    [Authorize]
    public class RepairJobsController : Controller
    {
        private readonly HttpClient _api;

        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public RepairJobsController(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("UstaApi");
        }

        // -------- LIST --------
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _api.GetAsync("repairjobs", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<RepairJobListDto>());
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<RepairJobListDto>>>(_json, ct);
            return View(wrap?.Data ?? new());
        }

        // -------- CREATE (GET) --------
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadVehicleOptionsAsync(ct, null);
            ViewBag.StatusOptions = BuildStatusOptions(null);

            return View(new RepairJobCreateDto());
        }

        // -------- CREATE (POST) --------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairJobCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            var resp = await _api.PostAsJsonAsync("repairjobs", dto, _json, ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            TempData["Success"] = "Tamir/Bakım başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // -------- EDIT (GET) --------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"repairjobs/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<RepairJobUpdateDto>>(_json, ct);
            var model = wrap?.Data;
            if (model is null)
            {
                TempData["Error"] = wrap?.Message ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            await LoadVehicleOptionsAsync(ct, model.VehicleId);
            ViewBag.StatusOptions = BuildStatusOptions(model.Status);

            return View(model);
        }

        // -------- EDIT (POST) --------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RepairJobUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            // API'de PUT /repairjobs şeklinde kullanıyorum
            var resp = await _api.PutAsJsonAsync("repairjobs", dto, _json, ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            TempData["Success"] = "Tamir/Bakım kaydı güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // -------- helpers --------

        private async Task LoadVehicleOptionsAsync(CancellationToken ct, Guid? selectedId = null)
        {
            var resp = await _api.GetAsync("vehicles/list", ct);
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.VehicleOptions = new List<SelectListItem>();
                return;
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleListDto>>>(_json, ct);
            var vehicles = wrap?.Data ?? new();

            ViewBag.VehicleOptions = vehicles.Select(v => new SelectListItem
            {
                Text = v.Plate,
                Value = v.Id.ToString(),
                Selected = selectedId.HasValue && v.Id == selectedId.Value
            }).ToList();
        }

        private static List<SelectListItem> BuildStatusOptions(string? selected = null)
        {
            var values = new[] { "Open", "InProgress", "Completed", "Cancelled" };
            return values.Select(v => new SelectListItem
            {
                Text = v,
                Value = v,
                Selected = string.Equals(v, selected, StringComparison.OrdinalIgnoreCase)
            }).ToList();
        }

        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                var pd = await resp.Content.ReadFromJsonAsync<ProblemDetails>(_json, ct);
                if (pd != null)
                {
                    var title = string.IsNullOrWhiteSpace(pd.Title) ? "Hata" : pd.Title;
                    var detail = string.IsNullOrWhiteSpace(pd.Detail) ? "" : $" - {pd.Detail}";
                    return $"API {(int)resp.StatusCode} {title}{detail}";
                }
            }
            catch { }

            var raw = await resp.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw)
                ? $"API {(int)resp.StatusCode} hatası (boş içerik)."
                : raw;
        }
    }
}
