using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Domain.Entities
{
    public class InsurancePolicy : IEntity
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty; // Allianz, Axa vs.
        public string PolicyNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal CoverageAmount { get; set; } // Maksimum ödeme limiti
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public ICollection<InsurancePayment> InsurancePayments { get; set; } = new List<InsurancePayment>();
    }

}
