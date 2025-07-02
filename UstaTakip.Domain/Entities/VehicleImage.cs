using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Domain.Entities
{
    public class VehicleImage : IEntity
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public string ImagePath { get; set; } = string.Empty; // örn: /images/vehicles/123abc.jpg
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

}
