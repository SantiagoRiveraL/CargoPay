using CargoPay.Core.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPay.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int userId);
        Task<bool> EmailExistsAsync(string email);
        Task<User> UpdateAsync(User user);
        Task<User> GetByResetTokenAsync(string token);

    }
}
