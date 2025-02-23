using CargoPay.Core.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.DTOs
{
    public class CardDto
    {
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
    }

    public class CardResponseDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }

    public class CreateCardRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 15)]
        [RegularExpression(@"^\d{15}$", ErrorMessage = "Card number must be exactly 15 digits")]
        public string CardNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal InitialBalance { get; set; }
    }

    public class PaymentRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 15)]
        public string CardNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
