using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakipMvc.Web.Models.VehicleImages;

namespace UstaTakipMvc.Web.Models.Vehicles
{
    public class VehicleListDto : IVehicleForm
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<VehicleImageListDto> Images { get; set; } = new();
    }

}
