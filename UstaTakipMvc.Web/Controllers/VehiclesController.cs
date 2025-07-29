using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using UstaTakipMvc.Web.Models.Customers;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly HttpClient _httpClient;
        public VehiclesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> IndexAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5280/api/Vehicles/list");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiDataResult<List<VehicleListDto>>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("http://localhost:5280/api/Customers");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiDataResult<List<CustomerListDto>>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ViewBag.Customers = result?.Data?.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("http://localhost:5280/api/Vehicles", jsonContent);
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiDataResult<object>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Başarısızsa: hata mesajı göster, müşteri listesini tekrar yükle
            if (!response.IsSuccessStatusCode || result?.Success != true)
            {
                // ✅ Müşteri listesini tekrar doldur
                var customerResponse = await _httpClient.GetAsync("http://localhost:5280/api/Customers");
                var customerJson = await customerResponse.Content.ReadAsStringAsync();
                var customerResult = JsonSerializer.Deserialize<ApiDataResult<List<CustomerListDto>>>(customerJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ViewBag.Customers = customerResult?.Data?.Select(c => new SelectListItem
                {
                    Text = c.FullName,
                    Value = c.Id.ToString()
                }).ToList();

                ViewBag.Error = result?.Message ?? "Araç eklenemedi.";
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5280/api/Vehicles/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiDataResult<VehicleUpdateDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || !result.Success)
            {
                ViewBag.Error = result?.Message ?? "Araç bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleUpdateDto dto)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync("http://localhost:5280/api/Vehicles", jsonContent);


            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API başarısız döndü.";
                return View(dto);
            }

            var json = await response.Content.ReadAsStringAsync();

            ApiDataResult<object>? result = null;
            if (!string.IsNullOrWhiteSpace(json))
            {
                result = JsonSerializer.Deserialize<ApiDataResult<object>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            if (result == null || result.Success == false)
            {
                ViewBag.Error = result?.Message ?? "Güncelleme başarısız.";
                return View(dto);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5280/api/Vehicles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Araç silinemedi.";
                return RedirectToAction("Index"); // Liste sayfasına yönlendir
            }

            TempData["Success"] = "Araç başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
