
namespace RCM.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        // Foreign key relationship
        public Role? Role { get; set; }

        // Forgot Password fields
        public string? PasswordResetTokenHash { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
