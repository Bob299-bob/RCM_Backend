
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using RCM.API.DTOs.Auth;
using RCM.API.Interfaces;

namespace RCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. USER REGISTRATION
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var message = await _userService.RegisterAsync(registerDto);

                return Ok(new { message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. USER LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDto);

                return Ok(new
                {
                    message = "Login successful.",
                    token = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // 3. FORGOT PASSWORD
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            try
            {
                await _userService.ForgotPasswordAsync(dto.Email);

                return Ok(new
                {
                    message = "If the account exists, a password reset link has been sent."
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Unable to process your request. Please try again later."
                });
            }
        }

        // 4. RESET PASSWORD
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _userService.ResetPasswordAsync(
                    dto.Token,
                    dto.NewPassword
                );

                return Ok(new
                {
                    message = "Password reset successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 5. AUTHENTICATED USER PROFILE
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                fullName,
                email,
                role,
                message = "You are authenticated successfully."
            });
        }

        // 6. ADMIN ONLY API
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok(new
            {
                message = "Welcome Admin! You have access to this API."
            });
        }

        // 7. SUBADMIN ONLY API
        [Authorize(Roles = "SubAdmin")]
        [HttpGet("subadmin-only")]
        public IActionResult SubAdminOnly()
        {
            return Ok(new
            {
                message = "Welcome SubAdmin! You have access to this API."
            });
        }

        // 8. USER ONLY API
        [Authorize(Roles = "User")]
        [HttpGet("user-only")]
        public IActionResult UserOnly()
        {
            return Ok(new
            {
                message = "Welcome User! You have access to this API."
            });
        }
    }
}
