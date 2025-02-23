using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Interfaces
{
    public interface IPaymentService
    {
        decimal CalculateFee();
        void UpdateFeeRate();
    }
}
