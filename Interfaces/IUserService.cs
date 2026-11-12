
using RCM.API.DTOs.Auth;

namespace RCM.API.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);

        Task<string> LoginAsync(LoginDto loginDto);

        // Forgot Password
        Task ForgotPasswordAsync(string email);

        // Reset Password
        Task ResetPasswordAsync(
            string token,
            string newPassword
        );
    }
}
