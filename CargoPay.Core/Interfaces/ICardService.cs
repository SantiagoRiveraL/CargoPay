using CargoPay.Core.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(int userId, string cardNumber, decimal initialBalance);
        Task<Card> GetCardByNumberAsync(string cardNumber);
        Task<Payment> ProcessPaymentAsync(int userId, string cardNumber, decimal amount);
        Task<decimal> GetBalanceAsync(int userId, string cardNumber);
    }
}
