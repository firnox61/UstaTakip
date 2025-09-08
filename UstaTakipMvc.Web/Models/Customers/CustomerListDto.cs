using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.Customers
{
    public class CustomerListDto 
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
