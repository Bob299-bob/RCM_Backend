
using RCM.API.DTOs.Auth;
using RCM.API.Interfaces;
using RCM.API.Models;

using System.Security.Cryptography;
using System.Text;

namespace RCM.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository userRepository,
            JwtService jwtService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        // Registration
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            string email = registerDto.Email.Trim().ToLowerInvariant();

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered."
                );
            }

            var role = await _userRepository.GetRoleByNameAsync("User");

            if (role == null)
            {
                throw new InvalidOperationException(
                    "Default User role was not found."
                );
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
                registerDto.Password
            );

            var user = new User
            {
                FullName = registerDto.FullName.Trim(),
                Email = email,
                PasswordHash = hashedPassword,
                RoleId = role.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            return "User registered successfully.";
        }

        // Login
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            string email = loginDto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password."
                );
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Your account is inactive."
                );
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password."
                );
            }

            string token = _jwtService.GenerateToken(user);

            return token;
        }

        // Forgot Password
        public async Task ForgotPasswordAsync(string email)
        {
            email = email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            // Do not reveal whether an email is registered
            if (user == null || !user.IsActive)
            {
                return;
            }

            // Generate secure random token
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);

            string resetToken = Convert.ToHexString(randomBytes);

            // Hash token before saving in database
            string tokenHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(resetToken))
            );

            user.PasswordResetTokenHash = tokenHash;

            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userRepository.UpdateAsync(user);

            // Create frontend reset link
            string resetLink =
                $"http://localhost:4200/reset-password?token={Uri.EscapeDataString(resetToken)}";

            // Send email
            try
            {
                await _emailService.SendPasswordResetEmailAsync(
                    user.Email,
                    resetLink
                );
            }
            catch
            {
                // Clear token if email sending fails
                user.PasswordResetTokenHash = null;
                user.PasswordResetTokenExpiry = null;

                await _userRepository.UpdateAsync(user);

                throw;
            }
        }

        // Reset Password
        public async Task ResetPasswordAsync(
            string token,
            string newPassword)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Reset token is required.");
            }

            if (string.IsNullOrWhiteSpace(newPassword) ||
                newPassword.Length < 8 ||
                newPassword.Length > 128)
            {
                throw new ArgumentException(
                    "Password must be between 8 and 128 characters."
                );
            }

            // Hash received token
            string tokenHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(token))
            );

            // Find user using token hash
            var user = await _userRepository.GetByResetTokenHashAsync(
                tokenHash
            );

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Invalid or expired reset link."
                );
            }

            // Check token expiry
            if (user.PasswordResetTokenExpiry == null ||
                user.PasswordResetTokenExpiry <= DateTime.UtcNow)
            {
                throw new InvalidOperationException(
                    "Invalid or expired reset link."
                );
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                newPassword
            );

            // Invalidate reset token
            user.PasswordResetTokenHash = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
        }
    }
}
