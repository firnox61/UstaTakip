using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.VehicleImages;

namespace UstaTakip.Application.Validators.Vehicles
{
    public class VehicleImageCreateDtoValidator : AbstractValidator<VehicleImageCreateDto>
    {
        public VehicleImageCreateDtoValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Araç ID boş olamaz.");

            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Bir görsel dosyası seçilmelidir.")
                .Must(file => file.Length > 0).WithMessage("Yüklenen dosya boş olamaz.")
                .Must(file => IsValidExtension(file.FileName)).WithMessage("Yalnızca .jpg, .jpeg veya .png dosyaları yüklenebilir.");
        }

        private bool IsValidExtension(string fileName)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(fileName)?.ToLower();
            return allowedExtensions.Contains(extension);
        }
    }


}
