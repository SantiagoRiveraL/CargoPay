using CargoPay.Core.Entitites;
using CargoPay.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CargoPay.Core.Interfaces;

namespace CargoPay.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class for managing user-related database operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly CargoPayDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">Database context for interacting with the database.</param>
        public UserRepository(CargoPayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user">The user entity to be created.</param>
        /// <returns>Returns the created user entity.</returns>
        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>Returns the user entity if found, otherwise null.</returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Cards)
                .Include(u => u.Payments)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns the user entity if found, otherwise null.</returns>
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Cards)
                .Include(u => u.Payments)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Checks if an email address already exists in the database.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>Returns true if the email exists, otherwise false.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated data.</param>
        /// <returns>Returns the updated user entity.</returns>
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Retrieves a user by their password reset token.
        /// </summary>
        /// <param name="token">The reset token.</param>
        /// <returns>Returns the user entity if found, otherwise null.</returns>
        public async Task<User> GetByResetTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }
    }
}