using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.RepairJobs
{
    public class RepairJobListDto:IDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public Guid VehicleId { get; set; }
        public string VehiclePlate { get; set; }
    }
}
