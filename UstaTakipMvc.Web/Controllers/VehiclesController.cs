using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using UstaTakipMvc.Web.Models.Customers;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;
using System.Net.Http.Headers;
using UstaTakipMvc.Web.Models.VehicleImages;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UstaTakipMvc.Web.Models.Customers;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;
using UstaTakipMvc.Web.Models.VehicleImages;

namespace UstaTakipMvc.Web.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly HttpClient _api;
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public VehiclesController(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("UstaApi");
        }

        // LIST
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _api.GetAsync("Vehicles/list", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return View(new List<VehicleListDto>());
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleListDto>>>(_json, ct);
            var list = wrapper?.Data ?? new List<VehicleListDto>();
            return View(list);
        }

        // CREATE (GET)
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await FillCustomersAsync(ct);
            return View(new VehicleCreateDto());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await FillCustomersAsync(ct);
                return View(dto);
            }

            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var resp = await _api.PostAsync("Vehicles", content, ct);

            if (!resp.IsSuccessStatusCode)
            {
                await FillCustomersAsync(ct);
                ViewBag.Error = await ReadApiErrorAsync(resp, ct);
                return View(dto);
            }

            var json = await resp.Content.ReadAsStringAsync(ct);
            var result = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<ApiDataResult<object>>(json, _json);

            if (result?.Success != true)
            {
                await FillCustomersAsync(ct);
                ViewBag.Error = result?.Message ?? "Araç eklenemedi.";
                return View(dto);
            }

            TempData["Success"] = "Araç başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            // 1) Araç
            var resp = await _api.GetAsync($"Vehicles/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<VehicleUpdateDto>>(_json, ct);
            if (wrapper is null || wrapper.Success != true || wrapper.Data is null)
            {
                TempData["Error"] = wrapper?.Message ?? "Araç bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Müşteriler
            await FillCustomersAsync(ct);

            // 3) Resimler
            var images = await GetVehicleImagesAsync(id, ct);
            ViewBag.VehicleImages = images;

            return View(wrapper.Data);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await FillCustomersAsync(ct);
                return View(dto);
            }

            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var resp = await _api.PutAsync("Vehicles", content, ct);

            if (!resp.IsSuccessStatusCode)
            {
                await FillCustomersAsync(ct);
                ViewBag.Error = await ReadApiErrorAsync(resp, ct);
                return View(dto);
            }

            var json = await resp.Content.ReadAsStringAsync(ct);
            var result = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<ApiDataResult<object>>(json, _json);

            if (result?.Success != true)
            {
                await FillCustomersAsync(ct);
                ViewBag.Error = result?.Message ?? "Güncelleme başarısız.";
                return View(dto);
            }

            TempData["Success"] = "Araç güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"Vehicles/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Araç başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ----------------- Vehicle Images -----------------

        private async Task<List<VehicleImageListDto>> GetVehicleImagesAsync(Guid vehicleId, CancellationToken ct)
        {
            var resp = await _api.GetAsync($"VehicleImages/by-vehicle/{vehicleId}", ct);
            if (!resp.IsSuccessStatusCode) return new();

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<VehicleImageListDto>>>(_json, ct);
            return wrapper?.Data ?? new();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(Guid vehicleId, IFormFile imageFile, CancellationToken ct)
        {
            if (vehicleId == Guid.Empty)
            {
                TempData["Error"] = "Araç bilgisi bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (imageFile is null || imageFile.Length == 0)
            {
                TempData["Error"] = "Yüklenecek dosya seçilmedi.";
                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            if (!imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Sadece görüntü dosyaları yüklenebilir.";
                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            const long maxBytes = 5 * 1024 * 1024;
            if (imageFile.Length > maxBytes)
            {
                TempData["Error"] = "Dosya boyutu 5MB’ı geçemez.";
                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            using var stream = imageFile.OpenReadStream();
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(vehicleId.ToString()), nameof(VehicleImageCreateDto.VehicleId));

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentLength = imageFile.Length;
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileContent, nameof(VehicleImageCreateDto.ImageFile), imageFile.FileName);

            var resp = await _api.PostAsync("VehicleImages", content, ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<object>>(_json, ct);
            if (wrapper?.Success != true)
                TempData["Error"] = wrapper?.Message ?? "Resim yüklenemedi.";
            else
                TempData["Success"] = "Resim yüklendi.";

            return RedirectToAction(nameof(Edit), new { id = vehicleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(Guid id, Guid vehicleId, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"VehicleImages/{id}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await ReadApiErrorAsync(resp, ct);
                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            TempData["Success"] = "Resim silindi.";
            return RedirectToAction(nameof(Edit), new { id = vehicleId });
        }

        // ----------------- helpers -----------------

        private async Task FillCustomersAsync(CancellationToken ct)
        {
            var resp = await _api.GetAsync("Customers", ct);
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Customers = new List<SelectListItem>();
                return;
            }

            var wrapper = await resp.Content.ReadFromJsonAsync<ApiDataResult<List<CustomerListDto>>>(_json, ct);
            var data = wrapper?.Data ?? new List<CustomerListDto>();

            ViewBag.Customers = data
                .Select(c => new SelectListItem
                {
                    Text = c.FullName,
                    Value = c.Id.ToString()
                })
                .ToList();
        }

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
