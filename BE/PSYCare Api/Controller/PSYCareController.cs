using backend.Domain;
using backend.Dtos;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PSYCareController : ControllerBase
    {
        private readonly IService _service;

        public PSYCareController(IService service)
        {
            _service = service;
        }

        [HttpPost("patients")]
        public IActionResult AddPatient([FromBody] PatientDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.PNC))
                return BadRequest("Patient name and PNC are required");

            var patient = new Patient { Name = dto.Name, Password = dto.Password };
            _service.AddPatient(patient, dto.PNC);
            return Ok(new { Message = "Patient added successfully", Patient = patient.Name });
        }

        [HttpGet("patients/{name}")]
        public IActionResult GetPatient(string name)
        {
            var patient = _service.GetPatient(name);
            if (patient == null) return NotFound("Patient not found");
            return Ok(patient);
        }

        [HttpGet("patients/{name}/pnc")]
        public IActionResult GetPatientPNC(string name)
        {
            var patient = _service.GetPatient(name);
            if (patient == null) return NotFound("Patient not found");

            var pnc = _service.GetPatientPNC(patient);
            return Ok(new { PNC = pnc });
        }

        [HttpPost("psychologists")]
        public IActionResult AddPsychologist([FromBody] PsychologistDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Psychologist name and code are required");

            var psych = new Psychologist(dto.Name, dto.Password, null);
            _service.AddPsychologist(psych, dto.Code);
            return Ok(new { Message = "Psychologist added successfully", Psychologist = psych.Name });
        }

        [HttpGet("psychologists/{name}")]
        public IActionResult GetPsychologist(string name)
        {
            var psych = _service.GetPsychologist(name);
            if (psych == null) return NotFound("Psychologist not found");
            return Ok(psych);
        }

        [HttpGet("psychologists/{name}/stamp")]
        public IActionResult GetPsychologistStamp(string name)
        {
            var psych = _service.GetPsychologist(name);
            if (psych == null) return NotFound("Psychologist not found");

            var stamp = _service.GetPsychologistStamp(psych);
            return Ok(new { StampCode = stamp });
        }

        [HttpPost("login/patient")]
        [AllowAnonymous]
        public IActionResult LoginPatient([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Name and password are required");

            var token = _service.LoginPatient(dto.Name, dto.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }

        [HttpPost("login/psychologist")]
        [AllowAnonymous]
        public IActionResult LoginPsychologist([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Name and password are required");

            var token = _service.LoginPsychologist(dto.Name, dto.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
        
        [HttpPost("Mood")]
        public IActionResult AddMood([FromBody] MoodDto dto)
        {
            var patient = _service.GetPatientById(dto.PatientId);
            if (patient == null) return NotFound("Patient not found");

            var mood = new Mood(patient, dto.Date, dto.Score);
            _service.AddMood(mood);

            return Ok("Mood added successfully");
        }

        [HttpGet("Mood/{patientId}")]
        public ActionResult<List<Mood>> GetMoods(int patientId)
        {
            var patient = _service.GetPatientById(patientId);
            if (patient == null) return NotFound("Patient not found");

            var moods = _service.GetMoods(patient);
            return Ok(moods);
        }
        
        [HttpPost("Appointment")]
        public IActionResult AddAppointment([FromBody] AppointmentDto dto)
        {
            var patient = _service.GetPatientById(dto.PatientId);
            if (patient == null) return NotFound("Patient not found");

            var psych = _service.GetPsychologistById(dto.PsychologistId);
            if (psych == null) return NotFound("Psychologist not found");

            var appointment = new Planificator
            {
                Patient = patient,
                PatientId = patient.Id,
                Psychologist = psych,
                PsychologistId = psych.Id,
                Date = dto.Date,
                Fee = dto.Fee
            };

            _service.AddAppointment(appointment);
            return Ok("Appointment added successfully");
        }

        [HttpGet("Appointment/Patient/{patientId}")]
        public ActionResult<List<Planificator>> GetPlanificatorsPatient(int patientId)
        {
            var patient = _service.GetPatientById(patientId);
            if (patient == null) return NotFound("Patient not found");

            var appointments = _service.GetPlanificatorsPatient(patient);
            return Ok(appointments);
        }

        [HttpGet("Appointment/Psychologist/{psychologistId}")]
        public ActionResult<List<Planificator>> GetPlanificatorsPsychologist(int psychologistId)
        {
            var psych = _service.GetPsychologistById(psychologistId);
            if (psych == null) return NotFound("Psychologist not found");

            var appointments = _service.GetPlanificatorsPsychologist(psych);
            return Ok(appointments);
        }
    }
}
