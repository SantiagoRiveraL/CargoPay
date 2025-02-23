using CargoPay.Core.Entitites;
using CargoPay.Core.Interfaces;
using CargoPay.Core.Services;
using CargoPay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for managing card-related database operations.
    /// </summary>
    public class CardRepository : ICardRepository
    {
        private readonly CargoPayDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CardRepository class.
        /// </summary>
        /// <param name="context">Database context for CargoPay.</param>
        public CardRepository(CargoPayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new card in the database.
        /// </summary>
        /// <param name="card">Card entity to be added.</param>
        /// <returns>Returns the created card.</returns>
        public async Task<Card> CreateCardAsync(Card card)
        {
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
        }

        /// <summary>
        /// Retrieves a card by its card number.
        /// </summary>
        /// <param name="cardNumber">The card number to search for.</param>
        /// <returns>Returns the matching card or null if not found.</returns>
        public async Task<Card> GetCardByNumberAsync(string cardNumber)
        {
            return await _context.Cards
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }

        /// <summary>
        /// Retrieves the balance of a specific card for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the card.</param>
        /// <param name="cardNumber">The card number to check the balance for.</param>
        /// <returns>Returns the balance amount or -1 if the card is not found.</returns>
        public async Task<decimal> GetBalanceAsync(int userId, string cardNumber)
        {
            var card = await _context.Cards
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber && c.UserId == userId);
            return card?.Balance ?? -1;
        }

        /// <summary>
        /// Updates an existing card in the database.
        /// </summary>
        /// <param name="card">The card entity with updated details.</param>
        public async Task UpdateCardAsync(Card card)
        {
            _context.Cards.Update(card);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves a payment transaction in the database.
        /// </summary>
        /// <param name="payment">Payment entity to be saved.</param>
        public async Task SavePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
