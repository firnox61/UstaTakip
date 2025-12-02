using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Domain.Entities
{
    public class RepairJobImage : IEntity
    {
        public Guid Id { get; set; }

        public Guid RepairJobId { get; set; }
        public RepairJob RepairJob { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    }
}
