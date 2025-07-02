using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Vehicles;

namespace UstaTakip.Application.Validators.Vehicles
{
    public class VehicleUpdateDtoValidator : AbstractValidator<VehicleUpdateDto>
    {
        public VehicleUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id boş olamaz.");

            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Plaka boş olamaz.")
                .MaximumLength(20).WithMessage("Plaka en fazla 20 karakter olabilir.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka boş olamaz.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model boş olamaz.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1950, DateTime.Now.Year + 1)
                .WithMessage($"Yıl 1950 ile {DateTime.Now.Year + 1} arasında olmalıdır.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri seçilmelidir.");
        }
    }
}
