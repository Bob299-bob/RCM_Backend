
using Microsoft.EntityFrameworkCore;
using RCM.API.Data;
using RCM.API.Interfaces;
using RCM.API.Models;

namespace RCM.API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Update patient
        public async Task<Patient?> UpdateAsync(int id, Patient updatedPatient)
        {
            var existingPatient = await GetByIdAsync(id);

            if (existingPatient == null)
            {
                return null;
            }

            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
            existingPatient.Gender = updatedPatient.Gender;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.Address = updatedPatient.Address;
            existingPatient.EmergencyContact = updatedPatient.EmergencyContact;

            await _context.SaveChangesAsync();

            return existingPatient;
        }

        // Delete patient
        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await GetByIdAsync(id);

            if (patient == null)
            {
                return false;
            }

            _context.Patients.Remove(patient);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
