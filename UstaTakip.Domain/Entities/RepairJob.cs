using UstaTakip.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakip.Domain.Entities
{
    public class RepairJob : IEntity
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        // Sigortanın ödeme oranı (%0, 25, 50, 75, 100)
        public int InsurancePaymentRate { get; set; } = 100;

        public Guid? InsurancePolicyId { get; set; }
        public InsurancePolicy? InsurancePolicy { get; set; }

        public ICollection<InsurancePayment> InsurancePayments { get; set; }
            = new List<InsurancePayment>();

        public string Status { get; set; } = "Open";

        public ICollection<RepairJobImage> RepairJobImages { get; set; }
            = new List<RepairJobImage>();
    }


}
