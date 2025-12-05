using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;
using UstaTakip.Domain.Enums;

namespace UstaTakip.Application.DTOs.RepairJobs
{

    public class RepairJobUpdateDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public Guid VehicleId { get; set; }

        public int InsurancePaymentRate { get; set; }
        public Guid? InsurancePolicyId { get; set; }

        public string Status { get; set; } = "Open";
    }

}
