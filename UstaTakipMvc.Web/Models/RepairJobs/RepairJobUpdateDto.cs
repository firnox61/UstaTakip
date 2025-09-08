using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.RepairJobs
{

    public class RepairJobUpdateDto 
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public Guid VehicleId { get; set; }
        public string Status { get; set; } = "Open"; // varsayılan
    }
}
