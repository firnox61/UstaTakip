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

        // API'ler genelde camelCase döner; case-insensitive profil:
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public RepairJobsController(IHttpClientFactory factory)
        {
            // Program.cs:
            // services.AddHttpClient("UstaApi", c => c.BaseAddress = new Uri("http://localhost:5280/api/"));
            _api = factory.CreateClient("UstaApi");
        }

        // -------- LIST (kısa tutuyoruz) --------
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

        // -------- EDIT --------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            // Kayıt
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

            // Select'leri doldur
            await LoadVehicleOptionsAsync(ct, model.VehicleId);           // plakalar
            ViewBag.StatusOptions = BuildStatusOptions(model.Status);     // durumlar

            return View(model);
        }

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

            var resp = await _api.PutAsJsonAsync($"repairjobs/{dto.Id}", dto, _json, ct);
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

        // -------- helpers (sade) --------

        // Araç listesi: vehicles/list -> VehicleListDto (Id, Plate ...). SelectListItem'a mapler.
        private async Task LoadVehicleOptionsAsync(CancellationToken ct, Guid? selectedId = null)
        {
            var resp = await _api.GetAsync("vehicles/list", ct); // API'nde "vehicles" ise onu yaz
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.VehicleOptions = new List<SelectListItem>();
                return;
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleListDto>>>(_json, ct);
            var vehicles = wrap?.Data ?? new();

            ViewBag.VehicleOptions = vehicles.Select(v => new SelectListItem
            {
                Text = v.Plate, // plaka göster
                Value = v.Id.ToString(),
                Selected = selectedId.HasValue && v.Id == selectedId.Value
            }).ToList();
        }

        // Durum select'i için SelectListItem üretir (tip uyumsuzluğu hatası yaşamazsın)
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

        // Anlaşılır hata metni
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
            catch { /* yutuyoruz, raw'a düşeceğiz */ }

            var raw = await resp.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw) ? $"API {(int)resp.StatusCode} hatası (boş içerik)." : raw;
        }
    }
}