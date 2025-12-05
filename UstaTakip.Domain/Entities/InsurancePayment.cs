using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Domain.Entities
{
    public class InsurancePayment : IEntity
    {
        public Guid Id { get; set; }

        public Guid RepairJobId { get; set; }
        public RepairJob RepairJob { get; set; }

        public Guid InsurancePolicyId { get; set; }
        public InsurancePolicy InsurancePolicy { get; set; }

        // ✔ Sigortanın ödeme oranı (%0 – %100)
        public int PaymentRate { get; set; }  // Örn: 0, 25, 50, 75, 100

        // ✔ Sigortanın gerçekte ödediği tutar
        public decimal PaidAmount { get; set; }

        public DateTime PaidDate { get; set; } = DateTime.UtcNow;

        public string Note { get; set; } = string.Empty;
    }



}
