using backend.Domain;
using backend.Dtos;
using backend.Repo;
using backend.Repo.Interfaces;

namespace backend.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepo;
        private readonly IPatientRepository patientRepo;
        private readonly IPsychologistRepository psychologistRepo;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            IPatientRepository patientRepo,
            IPsychologistRepository psychologistRepo)
        {
            this.appointmentRepo = appointmentRepo ?? throw new ArgumentNullException(nameof(appointmentRepo));
            this.patientRepo = patientRepo ?? throw new ArgumentNullException(nameof(patientRepo));
            this.psychologistRepo = psychologistRepo ?? throw new ArgumentNullException(nameof(psychologistRepo));
        }

        public void AddAppointment(AppointmentDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var patient = patientRepo.GetPatientById(dto.PatientId);
            if (patient == null) throw new Exception("Patient not found");

            var psychologist = psychologistRepo.GetPsychologistById(dto.PsychologistId);
            if (psychologist == null) throw new Exception("Psychologist not found");

            var appointment = new Planificator
            {
                Patient = patient,
                PatientId = patient.Id,
                Psychologist = psychologist,
                PsychologistId = psychologist.Id,
                Date = dto.Date,
                Fee = dto.Fee
            };

            appointmentRepo.AddAppointment(appointment);
        }

        public List<AppointmentResponseDto> GetPatientAppointments(int patientId)
        {
            var appointments = appointmentRepo.GetPatientAppointments(patientId);

            return appointments.Select(a => new AppointmentResponseDto
            {
                PsychologistId = a.PsychologistId,
                PsychologistName = a.Psychologist?.Name ?? "Unknown",
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name ?? "Unknown",
                Date = a.Date,
                Fee = a.Fee
            }).ToList();
        }

        public List<AppointmentResponseDto> GetPsychologistAppointments(int psychologistId)
        {
            var appointments = appointmentRepo.GetPsychologistAppointments(psychologistId);

            return appointments.Select(a => new AppointmentResponseDto
            {
                PsychologistId = a.PsychologistId,
                PsychologistName = a.Psychologist?.Name ?? "Unknown",
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name ?? "Unknown",
                Date = a.Date,
                Fee = a.Fee
            }).ToList();
        }
    }
}