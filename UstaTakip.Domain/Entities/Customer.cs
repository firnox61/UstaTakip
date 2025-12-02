using UstaTakip.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakip.Domain.Entities
{
    public class Customer : IEntity
    {
        public Guid Id { get; set; }

        // Bireysel mi, Tüzel mi?
        public CustomerType Type { get; set; }

        // Bireysel alanları
        public string? FullName { get; set; }      // sadece bireysel
        public string? NationalId { get; set; }    // TC kimlik

        // Tüzel alanları
        public string? CompanyName { get; set; }   // sadece tüzel
        public string? TaxNumber { get; set; }     // vergi no

        // Ortak alanlar
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }

    public enum CustomerType
    {
        Individual = 1,  // Bireysel
        Corporate = 2    // Tüzel
    }
}
