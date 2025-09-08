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
            // Program.cs: services.AddHttpClient("UstaApi", c => c.BaseAddress = new Uri("http://localhost:5280/api/"));
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

        // -------- EDIT (GET) --------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            // API tarafında: [HttpGet("{id:guid}")] => returns RepairJobUpdateDto
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

            // ✅ 405'i önlemek için API rotasıyla birebir uyum:
            // API'NDE ŞUNU KULLAN: [HttpPut("{id:guid}")]  (aşağıdaki satır doğru çağrı olur)
            var resp = await _api.PutAsJsonAsync("RepairJobs", dto, _json, ct);

            // Eğer API'n "idsiz" PUT kullanıyorsa ( [HttpPut] ):
            // var resp = await _api.PutAsJsonAsync("repairjobs", dto, _json, ct);

            // Eğer API "POST update" kullanıyorsa:
            // var resp = await _api.PostAsJsonAsync("repairjobs/update", dto, _json, ct);

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
            // API’de endpoint tam adı neyse onu yaz: "vehicles/list" veya "vehicles"
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
            catch { /* raw içerik okunacak */ }

            var raw = await resp.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw) ? $"API {(int)resp.StatusCode} hatası (boş içerik)." : raw;
        }
    }
}
