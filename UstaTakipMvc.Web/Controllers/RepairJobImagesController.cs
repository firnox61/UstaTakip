using Microsoft.AspNetCore.Mvc;

namespace UstaTakipMvc.Web.Controllers
{
    public class RepairJobImagesController : Controller
    {
        private readonly HttpClient _api;

        public RepairJobImagesController(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("UstaApi");
        }

        // UPLOAD (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Guid repairJobId, IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Dosya seçilmedi.";
                return RedirectToAction("Edit", "RepairJobs", new { id = repairJobId });
            }

            var form = new MultipartFormDataContent();
            form.Add(new StringContent(repairJobId.ToString()), "repairJobId");
            form.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

            var resp = await _api.PostAsync("RepairJobImages/upload", form, ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = "Resim yüklenemedi.";
            }
            else
            {
                TempData["Success"] = "Resim yüklendi.";
            }

            return RedirectToAction("Edit", "RepairJobs", new { id = repairJobId });
        }

        // DELETE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid repairJobId, CancellationToken ct)
        {
            var resp = await _api.DeleteAsync($"RepairJobImages/{id}", ct);

            if (!resp.IsSuccessStatusCode)
                TempData["Error"] = "Resim silinemedi.";
            else
                TempData["Success"] = "Resim silindi.";

            return RedirectToAction("Edit", "RepairJobs", new { id = repairJobId });
        }
    }
}

