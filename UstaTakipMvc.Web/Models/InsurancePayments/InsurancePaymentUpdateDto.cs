using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.InsurancePayments
{
    public class InsurancePaymentUpdateDto
    {
        public Guid Id { get; set; }

        public Guid RepairJobId { get; set; }
        public Guid InsurancePolicyId { get; set; }

        public int PaymentRate { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime PaidDate { get; set; }

        public string Note { get; set; } = "";
    }




}
