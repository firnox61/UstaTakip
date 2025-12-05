using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using UstaTakipMvc.Web.Models.InsurancePolicies;
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

        // ---------------- LIST ----------------
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
            var data = wrap?.Data ?? new();

            int StatusRank(string s) => s switch
            {
                "Open" => 1,
                "InProgress" => 2,
                "Completed" => 3,
                "Cancelled" => 4,
                _ => 99
            };

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

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            return View(data);
        }

        // ---------------- CREATE (GET) ----------------
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadVehicleOptionsAsync(ct);
            await LoadPolicyOptionsAsync(ct);

            ViewBag.StatusOptions = BuildStatusOptions(null);

            return View(new RepairJobCreateDto
            {
                Date = DateTime.Today,
                InsurancePaymentRate = 100
            });
        }

        // ---------------- CREATE (POST) ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairJobCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                await LoadPolicyOptionsAsync(ct, dto.InsurancePolicyId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            var resp = await _api.PostAsJsonAsync("repairjobs", dto, _json, ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                await LoadPolicyOptionsAsync(ct, dto.InsurancePolicyId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            TempData["Success"] = "İşlem başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- EDIT (GET) ----------------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"repairjobs/update-dto/{id}");


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
            await LoadPolicyOptionsAsync(ct, model.InsurancePolicyId);
            ViewBag.StatusOptions = BuildStatusOptions(model.Status);

            // Load images
            var imgResp = await _api.GetAsync($"repairjobimages/by-repair/{id}", ct);
            if (imgResp.IsSuccessStatusCode)
            {
                ViewBag.RepairJobImages =
                    await imgResp.Content.ReadFromJsonAsync<List<RepairJobImageListDto>>(_json, ct)
                    ?? new();
            }
            else
            {
                ViewBag.RepairJobImages = new List<RepairJobImageListDto>();
            }

            return View(model);
        }

        // ---------------- EDIT (POST) ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RepairJobUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                await LoadPolicyOptionsAsync(ct, dto.InsurancePolicyId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            var resp = await _api.PutAsJsonAsync("repairjobs", dto, _json, ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                await LoadVehicleOptionsAsync(ct, dto.VehicleId);
                await LoadPolicyOptionsAsync(ct, dto.InsurancePolicyId);
                ViewBag.StatusOptions = BuildStatusOptions(dto.Status);
                return View(dto);
            }

            TempData["Success"] = "Kayıt güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- DELETE ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"repairjobs/{id}", ct);

            TempData[resp.IsSuccessStatusCode ? "Success" : "Error"] =
                resp.IsSuccessStatusCode ? "Kayıt silindi." : await ReadApiErrorAsync(resp, ct);

            return RedirectToAction(nameof(Index));
        }

        // ---------------- HELPERS ----------------

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

        private async Task LoadPolicyOptionsAsync(CancellationToken ct, Guid? selectedId = null)
        {
            var resp = await _api.GetAsync("insurancepolicies", ct);
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.PolicyOptions = new List<SelectListItem>();
                return;
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<InsurancePolicyListDto>>>(_json, ct);
            var policies = wrap?.Data ?? new();

            ViewBag.PolicyOptions = policies.Select(p => new SelectListItem
            {
                Text = $"{p.CompanyName} - {p.PolicyNumber} ({p.VehiclePlate})",
                Value = p.Id.ToString(),
                Selected = selectedId.HasValue && p.Id == selectedId.Value
            }).ToList();
        }

        private List<SelectListItem> BuildStatusOptions(string? selected)
        {
            var statuses = new[] { "Open", "InProgress", "Completed", "Cancelled" };

            return statuses.Select(x => new SelectListItem
            {
                Text = x,
                Value = x,
                Selected = string.Equals(x, selected, StringComparison.OrdinalIgnoreCase)
            }).ToList();
        }

        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                var pd = await resp.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
                if (pd != null)
                {
                    return $"{pd.Title} - {pd.Detail}";
                }
            }
            catch { }

            return await resp.Content.ReadAsStringAsync(ct);
        }
    }
}
