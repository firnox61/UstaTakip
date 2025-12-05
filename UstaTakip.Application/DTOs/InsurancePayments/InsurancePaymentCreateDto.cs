using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.InsurancePayments
{
    public class InsurancePaymentCreateDto
    {
        public Guid RepairJobId { get; set; }
        public Guid InsurancePolicyId { get; set; }

        public int PaymentRate { get; set; }     // % oran
        public decimal PaidAmount { get; set; }  // Gerçek ödeme

        public DateTime PaidDate { get; set; } = DateTime.UtcNow;

        public string Note { get; set; } = string.Empty;
    }


}
