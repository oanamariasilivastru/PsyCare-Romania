using backend.Domain;

namespace backend.Repo.Interfaces;

public interface IAppointmentRepository
{
    void AddAppointment(Planificator appointment);
    List<Planificator> GetPatientAppointments(int patientId);
    List<Planificator> GetPsychologistAppointments(int psychologistId);
    Planificator? GetAppointmentById(int psychologistId, int patientId, DateTime date);
    void UpdateAppointment(Planificator appointment);
    void DeleteAppointment(int psychologistId, int patientId, DateTime date);
}