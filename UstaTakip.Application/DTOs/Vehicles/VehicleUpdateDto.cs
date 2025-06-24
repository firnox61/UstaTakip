using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.Vehicles
{
    public class VehicleUpdateDto : IDto
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Guid CustomerId { get; set; }
    }
}
