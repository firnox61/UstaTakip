using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.DTOs.Customers
{
    public class CustomerCreateDto : IDto
    {
        public CustomerType Type { get; set; }

        // Bireysel
        public string? FullName { get; set; }
        public string? NationalId { get; set; }

        // Tüzel
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }

        // Ortak
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
