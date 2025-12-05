using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.InsurancePolicies
{
    public class InsurancePolicyListDto
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal CoverageAmount { get; set; }

        public Guid VehicleId { get; set; }
        public string VehiclePlate { get; set; } = ""; // AutoMapper'da dolduruluyor
    }


}
