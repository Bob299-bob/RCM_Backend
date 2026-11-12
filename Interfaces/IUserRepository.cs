
using RCM.API.Models;

namespace RCM.API.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User> CreateAsync(User user);

        Task<Role?> GetRoleByNameAsync(string roleName);

        Task UpdateAsync(User user);

        // Forgot Password methods
        Task<User?> GetByResetTokenHashAsync(string tokenHash);
    }
}
