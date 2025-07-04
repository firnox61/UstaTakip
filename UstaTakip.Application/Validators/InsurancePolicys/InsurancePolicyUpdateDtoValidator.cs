using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePayments;
using UstaTakip.Application.DTOs.InsurancePolicys;

namespace UstaTakip.Application.Validators.InsurancePolicys
{
    public class InsurancePolicyUpdateDtoValidator : AbstractValidator<InsurancePolicyUpdateDto>
    {
        public InsurancePolicyUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Poliçe ID boş olamaz.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Sigorta şirketi adı boş olamaz.");

            RuleFor(x => x.PolicyNumber)
                .NotEmpty().WithMessage("Poliçe numarası boş olamaz.")
                .MaximumLength(50).WithMessage("Poliçe numarası en fazla 50 karakter olabilir.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");

            RuleFor(x => x.CoverageAmount)
                .GreaterThan(0).WithMessage("Kapsam tutarı sıfırdan büyük olmalıdır.");

            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Araç seçilmelidir.");
        }
    }



}
