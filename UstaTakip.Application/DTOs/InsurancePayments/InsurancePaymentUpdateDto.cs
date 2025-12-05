using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Abstractions;

namespace UstaTakip.Application.DTOs.InsurancePayments
{
    public class InsurancePaymentUpdateDto
    {
        public Guid Id { get; set; }

        public int PaymentRate { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }

        public string Note { get; set; } = string.Empty;
    }


}
