using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Validators.Customers
{
    public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id boş olamaz.");

            // Ortak alanlar
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .Matches(@"^\d{10,15}$").WithMessage("Telefon numarası 10-15 haneli rakam olmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş olamaz.");

            // Bireysel kurallar
            When(x => x.Type == CustomerType.Individual, () =>
            {
                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("Ad Soyad zorunludur.")
                    .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

                RuleFor(x => x.NationalId)
                    .NotEmpty().WithMessage("TC Kimlik No zorunludur.")
                    .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.");
            });

            // Tüzel kurallar
            When(x => x.Type == CustomerType.Corporate, () =>
            {
                RuleFor(x => x.CompanyName)
                    .NotEmpty().WithMessage("Şirket ünvanı zorunludur.")
                    .MaximumLength(150).WithMessage("Şirket ünvanı en fazla 150 karakter olabilir.");

                RuleFor(x => x.TaxNumber)
                    .NotEmpty().WithMessage("Vergi numarası zorunludur.")
                    .Length(10).WithMessage("Vergi numarası 10 haneli olmalıdır.");
            });
        }
    }
}
