using backend.Dtos;
using backend.Repo.Interfaces;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/PSYCare/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        }
        
        [HttpPost]
        public IActionResult AddAppointment([FromBody] AppointmentDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Appointment data is required");

                _appointmentService.AddAppointment(dto);
                return Ok(new { message = "Appointment added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add appointment", error = ex.Message });
            }
        }
        
        [HttpGet("Patient/{patientId}")]
        public ActionResult<List<AppointmentResponseDto>> GetPatientAppointments(int patientId)
        {
            try
            {
                var appointments = _appointmentService.GetPatientAppointments(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get appointments", error = ex.Message });
            }
        }
        
        [HttpGet("Psychologist/{psychologistId}")]
        public ActionResult<List<AppointmentResponseDto>> GetPsychologistAppointments(int psychologistId)
        {
            try
            {
                var appointments = _appointmentService.GetPsychologistAppointments(psychologistId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get appointments", error = ex.Message });
            }
        }
    }
}