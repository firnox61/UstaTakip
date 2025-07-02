using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UstaTakip.Application.DTOs.VehicleImages;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Domain.Entities;

namespace UstaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IVehicleImageService _vehicleImageService;
        private readonly IWebHostEnvironment _env;

        public VehicleImagesController(IVehicleImageService vehicleImageService, IWebHostEnvironment env)
        {
            _vehicleImageService = vehicleImageService;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] VehicleImageCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var extension = Path.GetExtension(dto.ImageFile.FileName);
            var fileName = Guid.NewGuid() + extension;
            var folderPath = Path.Combine(_env.WebRootPath, "images", "vehicles");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }

            var entity = new VehicleImage
            {
                Id = Guid.NewGuid(),
                VehicleId = dto.VehicleId,
                ImagePath = $"/images/vehicles/{fileName}",
                UploadedAt = DateTime.UtcNow
            };

            var result = await _vehicleImageService.AddAsync(entity);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var imageResult = await _vehicleImageService.GetByIdAsync(id);
            if (!imageResult.Success)
                return NotFound(imageResult.Message);

            var fullPath = Path.Combine(_env.WebRootPath, imageResult.Data.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            var result = await _vehicleImageService.DeleteAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}

