using CargoPay.Core.DTOs;
using CargoPay.Core.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(UserRegisterDto userDto);
        Task<AuthResponse> LoginAsync(UserLoginDto loginDto);
        string GenerateJwtToken(User user);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);

    }
}
