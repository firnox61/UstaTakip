using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.VehicleImages
{
    public class VehicleImageCreateDto
    {
        public Guid VehicleId { get; set; }
        public IFormFile ImageFile { get; set; }
    }

}
