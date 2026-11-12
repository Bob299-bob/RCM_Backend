
using RCM.API.DTOs.Patient;
using RCM.API.Interfaces;
using RCM.API.Models;

namespace RCM.API.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // Register a new patient
        public async Task<PatientResponseDto> CreateAsync(
            CreatePatientDto patientDto)
        {
            // Validate date of birth
            if (patientDto.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException(
                    "Date of birth cannot be in the future."
                );
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName.Trim(),
                LastName = patientDto.LastName.Trim(),
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender.Trim(),
                PhoneNumber = patientDto.PhoneNumber.Trim(),
                Email = patientDto.Email?.Trim(),
                Address = patientDto.Address?.Trim(),
                EmergencyContact = patientDto.EmergencyContact?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            var savedPatient = await _patientRepository.CreateAsync(patient);

            return MapToResponseDto(savedPatient);
        }

        // Update patient
        public async Task<PatientResponseDto?> UpdateAsync(
            int id,
            CreatePatientDto patientDto)
        {
            if (patientDto.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException(
                    "Date of birth cannot be in the future."
                );
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName.Trim(),
                LastName = patientDto.LastName.Trim(),
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender.Trim(),
                PhoneNumber = patientDto.PhoneNumber.Trim(),
                Email = patientDto.Email?.Trim(),
                Address = patientDto.Address?.Trim(),
                EmergencyContact = patientDto.EmergencyContact?.Trim()
            };

            var updatedPatient = await _patientRepository.UpdateAsync(id, patient);

            if (updatedPatient == null)
            {
                return null;
            }

            return MapToResponseDto(updatedPatient);
        }

        // Delete patient
        public async Task<bool> DeleteAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }


        // Get all patients
        public async Task<List<PatientResponseDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            return patients.Select(MapToResponseDto).ToList();
        }

        // Get patient by ID
        public async Task<PatientResponseDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                return null;
            }

            return MapToResponseDto(patient);
        }

        // Convert database model into response DTO
        private static PatientResponseDto MapToResponseDto(Patient patient)
        {
            return new PatientResponseDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContact = patient.EmergencyContact,
                CreatedAt = patient.CreatedAt
            };
        }
    }
}
