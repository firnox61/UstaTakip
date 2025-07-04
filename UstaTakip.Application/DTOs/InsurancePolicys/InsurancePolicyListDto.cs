using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.InsurancePolicys
{
    public class InsurancePolicyListDto:IDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal CoverageAmount { get; set; }
        public Guid VehicleId { get; set; }
    }

}
