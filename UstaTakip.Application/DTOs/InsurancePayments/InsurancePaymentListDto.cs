using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.InsurancePayments
{
    public class InsurancePaymentListDto
    {
        public Guid Id { get; set; }

        public Guid RepairJobId { get; set; }
        public string RepairJobDescription { get; set; } = string.Empty;

        public Guid InsurancePolicyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;

        public string VehiclePlate { get; set; } = string.Empty; // ← EKLENDİ

        public int PaymentRate { get; set; }
        public decimal PaidAmount { get; set; }

        public DateTime PaidDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }



}
