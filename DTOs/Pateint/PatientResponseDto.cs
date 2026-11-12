namespace RCM.API.DTOs.Patient
{
    public class PatientResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? EmergencyContact { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}