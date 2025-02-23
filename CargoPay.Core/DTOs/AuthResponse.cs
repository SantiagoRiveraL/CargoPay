using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
    }
}
