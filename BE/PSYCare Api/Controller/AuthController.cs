using backend.Domain;
using backend.Dtos;
using backend.Service;
using backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IPsychologistService _psychologistService;

        public AuthController(
            IPatientService patientService,
            IPsychologistService psychologistService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _psychologistService = psychologistService ?? throw new ArgumentNullException(nameof(psychologistService));
        }
        
        [HttpPost("patients")]
        public IActionResult AddPatient([FromBody] PatientDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.PNC))
                    return BadRequest("Patient name and PNC are required");

                var patient = new Patient { Name = dto.Name, Password = dto.Password };
                _patientService.AddPatient(patient, dto.PNC);
                return Ok(new { Message = "Patient added successfully", Patient = patient.Name });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add patient", error = ex.Message });
            }
        }

        [HttpGet("patients/{name}")]
        public IActionResult GetPatient(string name)
        {
            try
            {
                var patient = _patientService.GetPatient(name);
                if (patient == null) return NotFound("Patient not found");
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get patient", error = ex.Message });
            }
        }

        [HttpGet("patients/{name}/pnc")]
        public IActionResult GetPatientPNC(string name)
        {
            try
            {
                var patient = _patientService.GetPatient(name);
                if (patient == null) return NotFound("Patient not found");

                var pnc = _patientService.GetPatientPNC(patient);
                return Ok(new { PNC = pnc });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get PNC", error = ex.Message });
            }
        }
        
        [HttpPost("psychologists")]
        public IActionResult AddPsychologist([FromBody] PsychologistDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Code))
                    return BadRequest("Psychologist name and code are required");

                var psych = new Psychologist(dto.Name, dto.Password, null);
                _psychologistService.AddPsychologist(psych, dto.Code);
                return Ok(new { Message = "Psychologist added successfully", Psychologist = psych.Name });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add psychologist", error = ex.Message });
            }
        }

        [HttpGet("psychologists/{name}")]
        public IActionResult GetPsychologist(string name)
        {
            try
            {
                var psych = _psychologistService.GetPsychologist(name);
                if (psych == null) return NotFound("Psychologist not found");
                return Ok(psych);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get psychologist", error = ex.Message });
            }
        }

        [HttpGet("psychologists/{name}/stamp")]
        public IActionResult GetPsychologistStamp(string name)
        {
            try
            {
                var psych = _psychologistService.GetPsychologist(name);
                if (psych == null) return NotFound("Psychologist not found");

                var stamp = _psychologistService.GetPsychologistStamp(psych);
                return Ok(new { StampCode = stamp });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get stamp", error = ex.Message });
            }
        }
        
        [HttpPost("login/patient")]
        [AllowAnonymous]
        public IActionResult LoginPatient([FromBody] LoginDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Name and password are required");

                var token = _patientService.LoginPatient(dto.Name, dto.Password);
                if (token == null) return Unauthorized("Invalid credentials");

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Login failed", error = ex.Message });
            }
        }

        [HttpPost("login/psychologist")]
        [AllowAnonymous]
        public IActionResult LoginPsychologist([FromBody] LoginDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Name and password are required");

                var token = _psychologistService.LoginPsychologist(dto.Name, dto.Password);
                if (token == null) return Unauthorized("Invalid credentials");

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Login failed", error = ex.Message });
            }
        }
    }
}