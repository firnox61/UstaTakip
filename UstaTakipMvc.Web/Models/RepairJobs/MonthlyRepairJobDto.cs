using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.RepairJobs
{
    public class MonthlyRepairJobDto
    {
        public Guid RepairJobId { get; set; }

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        // Sigorta ödeme oranı (%0–100 arası)
        public int InsurancePaymentRate { get; set; }

        // Sigortadan ödenen toplam miktar (iş bazlı)
        public decimal TotalInsurancePaid { get; set; }

        // Müşteriye düşen tutar
        public decimal CustomerPayAmount => Price - TotalInsurancePaid;

        // Araç bilgisi (raporlama için)
        public string VehiclePlate { get; set; } = string.Empty;

        // Sigorta şirketi bilgisi
        public string CompanyName { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;
    }

}
