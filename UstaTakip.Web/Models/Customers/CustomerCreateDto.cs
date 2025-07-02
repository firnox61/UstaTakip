using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Web.Models.Customers
{
    public class CustomerCreateDto 
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
