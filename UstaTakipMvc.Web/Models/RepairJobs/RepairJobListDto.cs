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
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public Guid VehicleId { get; set; }
        public string VehiclePlate { get; set; }
        // Yeni alan
        public string Status { get; set; } = "Open"; // varsayılan

        
    }
}
