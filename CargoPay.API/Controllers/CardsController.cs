using CargoPay.Core.DTOs;
using CargoPay.Core.Entitites;
using CargoPay.Core.Extensions;
using CargoPay.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling card-related operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CardsController> _logger;

        /// <summary>
        /// Constructor for CardsController.
        /// </summary>
        /// <param name="cardService">Service for card operations.</param>
        public CardsController(ICardService cardService, ILogger<CardsController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new card for the authenticated user.
        /// </summary>
        /// <param name="request">Request data containing card number and initial balance.</param>
        /// <returns>Returns the created card details.</returns>
        [HttpPost]
        public async Task<ActionResult<CardResponseDto>> CreateCard([FromBody] CreateCardRequest request)
        {
            try
            {
                int userId = HttpContext.User.GetUserId();
                var card = await _cardService.CreateCardAsync(userId, request.CardNumber, request.InitialBalance);

                var cardDto = new CardResponseDto
                {
                    Id = card.Id,
                    CardNumber = card.CardNumber,
                    Balance = card.Balance,
                    CreatedAt = card.CreatedAt,
                    UserId = card.UserId
                };

                return CreatedAtAction(nameof(GetBalance), new { cardNumber = cardDto.CardNumber }, cardDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Authentication error: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid argument: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating card");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// Retrieves the balance of a specific card.
        /// </summary>
        /// <param name="cardNumber">The number of the card to check.</param>
        /// <returns>Returns the card balance information.</returns>
        [HttpGet("{cardNumber}/balance")]
        public async Task<ActionResult<CardDto>> GetBalance(string cardNumber)
        {
            try
            {
                int userId = HttpContext.User.GetUserId();
                var balance = await _cardService.GetBalanceAsync(userId, cardNumber);
                if (balance == -1)
                {
                    return NotFound(new { message = $"Card with number {cardNumber} has not been found in your account" });
                }

                return Ok(new CardDto { CardNumber = cardNumber, Balance = balance });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving balance");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// Processes a payment using a specified card.
        /// </summary>
        /// <param name="request">Request data containing card number and payment amount.</param>
        /// <returns>Returns payment transaction details.</returns>
        [HttpPost("payment")]
        public async Task<ActionResult<PaymentResponseDto>> ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                int userId = HttpContext.User.GetUserId();
                var paymentResult = await _cardService.ProcessPaymentAsync(userId, request.CardNumber, request.Amount);

                if (paymentResult == null)
                {
                    return BadRequest(new { message = "Payment could not be processed" });
                }

                return Ok(new PaymentResponseDto
                {
                    CardNumber = request.CardNumber,
                    Amount = request.Amount,
                    RemainingBalance = paymentResult.TotalAmount,
                    TransactionDate = paymentResult.TransactionDate
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Authentication error: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid argument: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }
}
