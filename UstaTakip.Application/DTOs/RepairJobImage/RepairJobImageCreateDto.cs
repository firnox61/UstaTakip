using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakip.Application.DTOs.RepairJobImage
{
    public class RepairJobImageCreateDto
    {
        [FromForm]
        public Guid RepairJobId { get; set; }

        [FromForm]
        public IFormFile File { get; set; }
    }

}
