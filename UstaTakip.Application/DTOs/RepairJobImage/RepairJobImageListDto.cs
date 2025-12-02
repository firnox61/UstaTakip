using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakip.Application.DTOs.RepairJobImage
{
    public class RepairJobImageListDto
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
