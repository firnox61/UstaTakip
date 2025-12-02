using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UstaTakip.Application.DTOs.RepairJobImage;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Domain.Entities;

namespace UstaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairJobImagesController : ControllerBase
    {
        private readonly IRepairJobImageService _service;
        private readonly IWebHostEnvironment _env;

        public RepairJobImagesController(IRepairJobImageService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] RepairJobImageCreateDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var extension = Path.GetExtension(dto.File.FileName);
            var fileName = Guid.NewGuid() + extension;

            var folderPath = Path.Combine(_env.WebRootPath, "images", "repairjobs");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await dto.File.CopyToAsync(stream);

            var entity = new RepairJobImage
            {
                Id = Guid.NewGuid(),
                RepairJobId = dto.RepairJobId,
                ImagePath = $"/images/repairjobs/{fileName}",
                UploadedAt = DateTime.UtcNow
            };

            var result = await _service.AddAsync(entity);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var imageResult = await _service.GetByIdAsync(id);
            if (!imageResult.Success)
                return NotFound(imageResult.Message);

            var fullPath = Path.Combine(_env.WebRootPath,
                imageResult.Data.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("by-repair/{repairJobId}")]
        public async Task<IActionResult> GetByRepairJob(Guid repairJobId)
        {
            var list = await _service.GetByRepairJobIdAsync(repairJobId);
            return list.Success ? Ok(list.Data) : BadRequest(list.Message);
        }

    }
}
