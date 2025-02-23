using CargoPay.Core.Entitites;
using CargoPay.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CargoPay.Core.Services
{
    /// <summary>
    /// Service responsible for handling card-related operations.
    /// </summary>
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentService _feeService;
        private readonly SemaphoreSlim _paymentSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the CardService class.
        /// </summary>
        /// <param name="cardRepository">Repository for card data operations.</param>
        /// <param name="userRepository">Repository for user data operations.</param>
        public CardService(ICardRepository cardRepository, IUserRepository userRepository)
        {
            _cardRepository = cardRepository;
            _userRepository = userRepository;
            _feeService = PaymentFeeService.Instance;
        }

        /// <summary>
        /// Creates a new card for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the card.</param>
        /// <param name="cardNumber">The card number to be assigned.</param>
        /// <param name="initialBalance">The initial balance of the card.</param>
        /// <returns>Returns the created card entity.</returns>
        /// <exception cref="ArgumentException">Thrown when the card number is invalid or the user is not found.</exception>
        public async Task<Card> CreateCardAsync(int userId, string cardNumber, decimal initialBalance)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 15)
                throw new ArgumentException("Card number must be 15 digits");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var existCard = await _cardRepository.GetCardByNumberAsync(cardNumber);
            if (existCard != null)
            {
                throw new ArgumentException("Card already exists");
            }

            var card = new Card
            {
                CardNumber = cardNumber,
                Balance = initialBalance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };

            return await _cardRepository.CreateCardAsync(card);
        }

        /// <summary>
        /// Retrieves a card by its card number.
        /// </summary>
        /// <param name="cardNumber">The card number to search for.</param>
        /// <returns>Returns the card entity or null if not found.</returns>
        public async Task<Card> GetCardByNumberAsync(string cardNumber)
        {
            return await _cardRepository.GetCardByNumberAsync(cardNumber);
        }

        /// <summary>
        /// Retrieves the balance of a specific card for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the card.</param>
        /// <param name="cardNumber">The card number to check the balance for.</param>
        /// <returns>Returns the balance amount.</returns>
        public async Task<decimal> GetBalanceAsync(int userId, string cardNumber)
        {
            return await _cardRepository.GetBalanceAsync(userId, cardNumber);
        }

        /// <summary>
        /// Processes a payment transaction using a specified card.
        /// </summary>
        /// <param name="userId">The ID of the user making the payment.</param>
        /// <param name="cardNumber">The card number used for the transaction.</param>
        /// <param name="amount">The payment amount.</param>
        /// <returns>Returns the payment transaction details if successful, otherwise null.</returns>
        public async Task<Payment> ProcessPaymentAsync(int userId, string cardNumber, decimal amount)
        {
            try
            {
                await _paymentSemaphore.WaitAsync();

                var card = await _cardRepository.GetCardByNumberAsync(cardNumber);
                if (card == null) return null;

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return null;

                var fee = _feeService.CalculateFee();
                var totalAmount = amount + fee;

                if (card.Balance < totalAmount)
                    return null;

                var payment = new Payment
                {
                    CardId = card.Id,
                    UserId = userId,
                    Amount = amount,
                    Fee = fee,
                    TotalAmount = totalAmount,
                    TransactionDate = DateTime.UtcNow,
                    Card = card,
                    User = user
                };

                card.Balance -= totalAmount;
                card.UpdatedAt = DateTime.UtcNow;

                await _cardRepository.UpdateCardAsync(card);
                await _cardRepository.SavePaymentAsync(payment);

                return payment;
            }
            finally
            {
                _paymentSemaphore.Release();
            }
        }
    }
}
