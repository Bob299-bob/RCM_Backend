
using RCM.API.Models;

namespace RCM.API.Interfaces
{
    public interface IPatientRepository
    {
        // Create a new patient
        Task<Patient> CreateAsync(Patient patient);

        // Get all patients
        Task<List<Patient>> GetAllAsync();

        // Get patient by ID
        Task<Patient?> GetByIdAsync(int id);

        // Update patient
        Task<Patient?> UpdateAsync(int id, Patient patient);

        // Delete patient
        Task<bool> DeleteAsync(int id);
    }
}
