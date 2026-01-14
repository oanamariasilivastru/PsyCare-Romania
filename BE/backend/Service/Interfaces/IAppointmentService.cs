using backend.Dtos;

namespace backend.Repo.Interfaces;

public interface IAppointmentService
{
    void AddAppointment(AppointmentDto appointmentDto);
    List<AppointmentResponseDto> GetPatientAppointments(int patientId);
    List<AppointmentResponseDto> GetPsychologistAppointments(int psychologistId);
}