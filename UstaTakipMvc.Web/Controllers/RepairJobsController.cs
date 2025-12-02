using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using UstaTakipMvc.Web.Models.RepairJobImages;
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
        public async Task<IActionResult> Index(string sortField, string sortOrder = "asc", CancellationToken ct = default)
        {
            var resp = await _api.GetAsync("repairjobs", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<RepairJobListDto>());
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<RepairJobListDto>>>(_json, ct);
            var data = wrap?.Data ?? new List<RepairJobListDto>();

            // -------- STATUS özel sıralama fonksiyonu --------
            int StatusRank(string s) => s switch
            {
                "Open" => 1,
                "InProgress" => 2,
                "Completed" => 3,
                "Cancelled" => 4,
                _ => 99
            };

            // -------- Sıralama işlemi --------
            data = sortField switch
            {
                "Plate" => sortOrder == "asc"
                    ? data.OrderBy(x => x.VehiclePlate).ToList()
                    : data.OrderByDescending(x => x.VehiclePlate).ToList(),

                "Description" => sortOrder == "asc"
                    ? data.OrderBy(x => x.Description).ToList()
                    : data.OrderByDescending(x => x.Description).ToList(),

                "Price" => sortOrder == "asc"
                    ? data.OrderBy(x => x.Price).ToList()
                    : data.OrderByDescending(x => x.Price).ToList(),

                "Date" => sortOrder == "asc"
                    ? data.OrderBy(x => x.Date).ToList()
                    : data.OrderByDescending(x => x.Date).ToList(),

                "Status" => sortOrder == "asc"
                    ? data.OrderBy(x => StatusRank(x.Status)).ToList()
                    : data.OrderByDescending(x => StatusRank(x.Status)).ToList(),

                _ => data
            };

            // -------- ViewBag ile mevcut sıralamayı taşıyalım --------
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            return View(data);
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

            // Araç listesi
            await LoadVehicleOptionsAsync(ct, model.VehicleId);

            // Status listesi
            ViewBag.StatusOptions = BuildStatusOptions(model.Status);

            // --- REPAIR JOB IMAGES (Galeri) ---
            var imgResp = await _api.GetAsync($"repairjobimages/by-repair/{id}", ct);
            if (imgResp.IsSuccessStatusCode)
            {
                var wrapImages = await imgResp.Content.ReadFromJsonAsync<List<RepairJobImageListDto>>(_json, ct);
                ViewBag.RepairJobImages = wrapImages ?? new();
            }
            else
            {
                ViewBag.RepairJobImages = new List<RepairJobImageListDto>();
            }


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

        // -------- DELETE --------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"repairjobs/{id}", ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
            }
            else
            {
                TempData["Success"] = "Kayıt başarıyla silindi.";
            }

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
