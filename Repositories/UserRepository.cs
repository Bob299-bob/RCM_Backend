
using Microsoft.EntityFrameworkCore;
using RCM.API.Data;
using RCM.API.Interfaces;
using RCM.API.Models;

namespace RCM.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Find user by email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Save new user
        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByResetTokenHashAsync(string tokenHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.PasswordResetTokenHash == tokenHash);
        }

    }
}
