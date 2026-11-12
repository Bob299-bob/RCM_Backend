
using RCM.API.DTOs.Patient;

namespace RCM.API.Interfaces
{
    public interface IPatientService
    {
        Task<PatientResponseDto> CreateAsync(CreatePatientDto patientDto);

        Task<List<PatientResponseDto>> GetAllAsync();

        Task<PatientResponseDto?> GetByIdAsync(int id);

        // Update patient
        Task<PatientResponseDto?> UpdateAsync(
            int id,
            CreatePatientDto patientDto
        );

        // Delete patient
        Task<bool> DeleteAsync(int id);
    }
}
