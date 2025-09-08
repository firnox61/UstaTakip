using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UstaTakipMvc.Web.Models.Customers;
using UstaTakipMvc.Web.Models.Shared;

namespace UstaTakipMvc.Web.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly HttpClient _api;
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomersController(IHttpClientFactory factory)
        {
            // Program.cs: services.AddHttpClient("UstaApi", c => c.BaseAddress = new Uri("http://localhost:5280/api/"));
            _api = factory.CreateClient("UstaApi");
        }

        // LIST
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _api.GetAsync("Customers", ct); // veya "Customers/list" senin API'ne göre
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<CustomerListDto>());
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<CustomerListDto>>>(_json, ct);
            var list = wrapper?.Data ?? new();
            return View(list);
        }

        // CREATE (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CustomerCreateDto());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var resp = await _api.PostAsync("Customers", content, ct);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = await ReadApiErrorAsync(resp, ct);
                return View(dto);
            }

            var json = await resp.Content.ReadAsStringAsync(ct);
            var result = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<ApiDataResult<object>>(json, _json);

            if (result?.Success != true)
            {
                ViewBag.Error = result?.Message ?? "Müşteri eklenemedi.";
                return View(dto);
            }

            TempData["Success"] = "Müşteri eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"Customers/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<CustomerUpdateDto>>(_json, ct);
            if (wrapper is null || wrapper.Success != true || wrapper.Data is null)
            {
                TempData["Error"] = wrapper?.Message ?? "Müşteri bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(wrapper.Data);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var resp = await _api.PutAsync("Customers", content, ct);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = await ReadApiErrorAsync(resp, ct);
                return View(dto);
            }

            var json = await resp.Content.ReadAsStringAsync(ct);
            var result = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<ApiDataResult<object>>(json, _json);

            if (result?.Success != true)
            {
                ViewBag.Error = result?.Message ?? "Güncelleme başarısız.";
                return View(dto);
            }

            TempData["Success"] = "Müşteri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"Customers/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Müşteri silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ----------------- helpers -----------------
        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            try
            {
                var parsed = JsonSerializer.Deserialize<ApiDataResult<object>>(body, _json);
                return !string.IsNullOrWhiteSpace(parsed?.Message)
                    ? parsed.Message
                    : $"API hata ({(int)resp.StatusCode})";
            }
            catch
            {
                return $"API hata ({(int)resp.StatusCode})";
            }
        }
    }
}
