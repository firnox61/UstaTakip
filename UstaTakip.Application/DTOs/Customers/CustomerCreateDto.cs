using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.Customers
{
    public class CustomerCreateDto : IDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
