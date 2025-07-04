using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.InsurancePayments
{
    public class InsurancePaymentUpdateDto:IDto
    {
        public Guid Id { get; set; }
        public Guid RepairJobId { get; set; }
        public Guid InsurancePolicyId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
