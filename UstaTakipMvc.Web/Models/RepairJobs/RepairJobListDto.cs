using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.RepairJobs
{
    public class RepairJobListDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public string Status { get; set; } = "Open";

        public int InsurancePaymentRate { get; set; }

        public string VehiclePlate { get; set; } = string.Empty;

        public string? CompanyName { get; set; }
        public string? PolicyNumber { get; set; }
        public string? AgencyCode { get; set; }
    }
}
