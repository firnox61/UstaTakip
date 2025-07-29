using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.VehicleImages
{
    public class VehicleImageListDto
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public string ImagePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }


}
