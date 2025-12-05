using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.InsurancePayments
{
    public class InsurancePaymentListDto
    {
        public Guid Id { get; set; }

        public Guid RepairJobId { get; set; }
        public string RepairJobDescription { get; set; } = "";

        public Guid InsurancePolicyId { get; set; }

        // Araç bilgisi
        public string VehiclePlate { get; set; } = "";

        // Sigorta poliçe bilgisi
        public string CompanyName { get; set; } = "";
        public string PolicyNumber { get; set; } = "";
        public string AgencyCode { get; set; } = "";

        // Sigorta ödeme oranı
        public int PaymentRate { get; set; }

        // Ödenen tutar
        public decimal PaidAmount { get; set; }

        public DateTime PaidDate { get; set; }

        // Not
        public string Note { get; set; } = "";
    }



}
