using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UstaTakipMvc.Web.Models.InsurancePayments;
using UstaTakipMvc.Web.Models.InsurancePolicies;
using UstaTakipMvc.Web.Models.RepairJobs;
using UstaTakipMvc.Web.Models.Shared;

namespace UstaTakipMvc.Web.Controllers
{
    [Authorize]
    public class InsurancePaymentsController : Controller
    {
        private readonly HttpClient _api;

        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public InsurancePaymentsController(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("UstaApi");
        }

        // LIST -----------------------------------------------------------
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _api.GetAsync("insurancepayments", ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<InsurancePaymentListDto>());
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<InsurancePaymentListDto>>>(_json, ct);
            var list = wrap?.Success == true ? wrap.Data ?? new() : new();

            return View(list);
        }

        // CREATE ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadRepairJobs(ct);
            await LoadPolicies(ct);
            return View(new InsurancePaymentCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsurancePaymentCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadRepairJobs(ct, dto.RepairJobId);
                await LoadPolicies(ct, dto.InsurancePolicyId);
                return View(dto);
            }

            var resp = await _api.PostAsJsonAsync("insurancepayments", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message;
                await LoadRepairJobs(ct, dto.RepairJobId);
                await LoadPolicies(ct, dto.InsurancePolicyId);
                return View(dto);
            }

            TempData["Success"] = "Ödeme kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT ------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"insurancepayments/{id}", ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<InsurancePaymentUpdateDto>>(_json, ct);
            var model = wrap?.Data;

            if (model == null)
            {
                TempData["Error"] = wrap?.Message ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            await LoadRepairJobs(ct, model.RepairJobId);
            await LoadPolicies(ct, model.InsurancePolicyId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InsurancePaymentUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadRepairJobs(ct, dto.RepairJobId);
                await LoadPolicies(ct, dto.InsurancePolicyId);
                return View(dto);
            }

            var resp = await _api.PutAsJsonAsync("insurancepayments", dto, _json, ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message;
                await LoadRepairJobs(ct, dto.RepairJobId);
                await LoadPolicies(ct, dto.InsurancePolicyId);
                return View(dto);
            }

            TempData["Success"] = "Ödeme güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE ----------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"insurancepayments/{id}", ct);
            var ok = await ReadResultEnvelope(resp, ct);

            if (!ok.success)
            {
                TempData["Error"] = ok.message;
            }
            else
            {
                TempData["Success"] = "Ödeme silindi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // HELPERS ---------------------------------------------------------
        private async Task LoadRepairJobs(CancellationToken ct, Guid? selected = null)
        {
            var resp = await _api.GetAsync("repairjobs", ct);
            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<RepairJobListDto>>>(_json, ct);

            ViewBag.RepairJobs = wrap?.Data ?? new();
            ViewBag.SelectedRepairJob = selected;
        }

        private async Task LoadPolicies(CancellationToken ct, Guid? selected = null)
        {
            var resp = await _api.GetAsync("insurancepolicies", ct);
            var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<InsurancePolicyListDto>>>(_json, ct);

            ViewBag.Policies = wrap?.Data ?? new();
            ViewBag.SelectedPolicy = selected;
        }

        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<object>>(cancellationToken: ct);
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
                var wrap = await resp.Content.ReadFromJsonAsync<ApiDataResult<object>>(cancellationToken: ct);
                if (wrap != null)
                    return (wrap.Success, wrap.Message);
            }
            catch { }

            return (resp.IsSuccessStatusCode, null);
        }
    }
}
