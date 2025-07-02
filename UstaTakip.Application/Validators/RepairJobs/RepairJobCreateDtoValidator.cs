using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;

namespace UstaTakip.Application.Validators.RepairJobs
{
    public class RepairJobCreateDtoValidator : AbstractValidator<RepairJobCreateDto>
    {
        public RepairJobCreateDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Tarih alanı boş olamaz.");

            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Araç ID boş olamaz.");
        }
    }

}
