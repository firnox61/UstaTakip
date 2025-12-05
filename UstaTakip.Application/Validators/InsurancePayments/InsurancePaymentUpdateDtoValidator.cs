using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePayments;

namespace UstaTakip.Application.Validators.InsurancePayments
{
    public class InsurancePaymentUpdateDtoValidator : AbstractValidator<InsurancePaymentUpdateDto>
    {
       /* public InsurancePaymentUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Ödeme ID boş olamaz.");

            RuleFor(x => x.RepairJobId)
                .NotEmpty().WithMessage("Onarım işlemi seçilmelidir.");

            RuleFor(x => x.InsurancePolicyId)
                .NotEmpty().WithMessage("Sigorta poliçesi seçilmelidir.");

            RuleFor(x => x.PaidAmount)
                .GreaterThan(0).WithMessage("Ödenen tutar sıfırdan büyük olmalıdır.");

            RuleFor(x => x.PaidDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Ödeme tarihi bugünden ileri olamaz.");
        }*/
    }


}
