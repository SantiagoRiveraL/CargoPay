using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.DTOs
{
    public class PaymentResponseDto
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingBalance { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
