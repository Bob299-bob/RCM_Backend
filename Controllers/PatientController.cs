
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using RCM.API.DTOs.Patient;
using RCM.API.Interfaces;

namespace RCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // POST: api/Patient
        // Admin and SubAdmin can register patients
        [HttpPost]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> CreatePatient(
            CreatePatientDto patientDto)
        {
            try
            {
                var result = await _patientService.CreateAsync(patientDto);

                return CreatedAtAction(
                    nameof(GetPatientById),
                    new { id = result.Id },
                    result
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/Patient
        // Admin, SubAdmin and User can view all patients
        [HttpGet]
        [Authorize(Roles = "Admin,SubAdmin,User")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllAsync();

            return Ok(patients);
        }

        // GET: api/Patient/1
        // Admin, SubAdmin and User can view patient details
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,SubAdmin,User")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Patient ID must be greater than zero."
                });
            }

            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(patient);
        }

        // PUT: api/Patient/2
        // Admin and SubAdmin can update patients
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            CreatePatientDto patientDto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Patient ID must be greater than zero."
                });
            }

            try
            {
                var result = await _patientService.UpdateAsync(
                    id,
                    patientDto
                );

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Patient not found."
                    });
                }

                return Ok(new
                {
                    message = "Patient updated successfully.",
                    patient = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Patient/2
        // Only Admin can delete patients
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Patient ID must be greater than zero."
                });
            }

            var deleted = await _patientService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(new
            {
                message = "Patient deleted successfully."
            });
        }
    }
}
