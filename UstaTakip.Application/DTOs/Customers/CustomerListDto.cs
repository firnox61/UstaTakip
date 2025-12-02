using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.DTOs.Customers
{
    public class CustomerListDto : IDto
    {
        public Guid Id { get; set; }
        public CustomerType Type { get; set; }

        // Görüntülenebilir isim
        public string DisplayName { get; set; } = string.Empty;

        // Ortak
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
