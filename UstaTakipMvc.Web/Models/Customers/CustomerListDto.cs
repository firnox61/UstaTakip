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
        public CustomerType Type { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
