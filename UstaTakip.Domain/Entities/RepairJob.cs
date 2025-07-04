using UstaTakip.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakip.Domain.Entities
{
    public class RepairJob : IEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public InsurancePayment InsurancePayment { get; set; }

    }

}
