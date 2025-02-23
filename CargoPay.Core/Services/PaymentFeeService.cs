using CargoPay.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Services
{
    /// <summary>
    /// Singleton service responsible for calculating payment fees dynamically.
    /// </summary>
    public class PaymentFeeService : IPaymentService
    {
        private static readonly object _lock = new object();
        private static PaymentFeeService _instance;
        private decimal _currentFeeRate = 1.0M;
        private decimal _lastFeeAmount = 1.0M;
        private readonly Random _random;
        private DateTime _lastUpdate;

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// Initializes the random generator and updates the initial fee rate.
        /// </summary>
        private PaymentFeeService()
        {
            _random = new Random();
            _lastUpdate = DateTime.UtcNow;
            UpdateFeeRate(); // Initialize with the first fee rate.
        }

        /// <summary>
        /// Gets the singleton instance of the PaymentFeeService.
        /// </summary>
        public static PaymentFeeService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new PaymentFeeService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Calculates and returns the current payment fee rate.
        /// </summary>
        /// <returns>The current fee rate as a decimal value.</returns>
        public decimal CalculateFee()
        {
            lock (_lock)
            {
                CheckAndUpdateFee(); // Verify if the fee needs to be updated.
                return _currentFeeRate;
            }
        }

        /// <summary>
        /// Checks if the fee rate needs to be updated based on elapsed time.
        /// </summary>
        private void CheckAndUpdateFee()
        {
            var currentTime = DateTime.UtcNow;
            // If debugging is needed, the function should be placed in TotalMinutes.
            if (currentTime.Subtract(_lastUpdate).TotalMinutes >= 1)
            {
                UpdateFeeRate();
                _lastUpdate = currentTime;
            }
        }

        /// <summary>
        /// Updates the fee rate with a new randomly generated multiplier.
        /// </summary>
        public void UpdateFeeRate()
        {
            lock (_lock)
            {
                decimal randomMultiplier = (decimal)(_random.NextDouble() * 2);
                _lastFeeAmount = _currentFeeRate;
                _currentFeeRate = _lastFeeAmount * randomMultiplier;
            }
        }
    }
}