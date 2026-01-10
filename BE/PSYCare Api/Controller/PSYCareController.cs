using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using backend.Domain;
using backend.Dtos;
using backend.Service;
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

        // ========================
        // Patients Endpoints
        // ========================

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

        // ========================
        // Psychologists Endpoints
        // ========================

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
    }
}
