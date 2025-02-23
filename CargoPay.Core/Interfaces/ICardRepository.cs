using CargoPay.Core.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<Card> CreateCardAsync(Card card);
        Task<Card> GetCardByNumberAsync(string cardNumber);
        Task<decimal> GetBalanceAsync(int userId, string cardNumber);
        Task UpdateCardAsync(Card card);
        Task SavePaymentAsync(Payment payment);

    }
}
