using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using UstaTakipMvc.Web.Models.VehicleImages;

namespace UstaTakipMvc.Web.Controllers
{
    public class VehicleImagesController : Controller
    {
        private readonly HttpClient _httpClient;

        public VehicleImagesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Upload(Guid vehicleId)
        {
            ViewBag.VehicleId = vehicleId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(VehicleImageCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                ViewBag.Error = "Lütfen bir dosya seçin.";
                ViewBag.VehicleId = dto.VehicleId;
                return View();
            }

            var form = new MultipartFormDataContent();
            var streamContent = new StreamContent(dto.ImageFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ImageFile.ContentType);
            form.Add(streamContent, "ImageFile", dto.ImageFile.FileName);
            form.Add(new StringContent(dto.VehicleId.ToString()), "VehicleId");

            var response = await _httpClient.PostAsync("http://localhost:5280/api/VehicleImages/upload", form);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = responseText;
                return RedirectToAction("Index", "Vehicles");
            }

            ViewBag.Error = responseText;
            ViewBag.VehicleId = dto.VehicleId;
            return View();
        }
    }
}
