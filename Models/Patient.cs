
using System.ComponentModel.DataAnnotations;

namespace RCM.API.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
        [Required]
        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? EmergencyContact { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
