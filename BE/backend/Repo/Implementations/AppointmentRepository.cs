using backend.Domain;
using backend.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repo
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PSYCareDbContext dbContext;

        public AppointmentRepository(PSYCareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void AddAppointment(Planificator appointment)
        {
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));
            dbContext.Planificators.Add(appointment);
            dbContext.SaveChanges();
        }

        public List<Planificator> GetPatientAppointments(int patientId)
        {
            return dbContext.Planificators
                .Include(p => p.Psychologist)
                .Where(p => p.PatientId == patientId)
                .OrderBy(p => p.Date)
                .ToList();
        }

        public List<Planificator> GetPsychologistAppointments(int psychologistId)
        {
            return dbContext.Planificators
                .Include(p => p.Patient)
                .Where(p => p.PsychologistId == psychologistId)
                .OrderBy(p => p.Date)
                .ToList();
        }

        public Planificator? GetAppointmentById(int psychologistId, int patientId, DateTime date)
        {
            return dbContext.Planificators
                .Include(p => p.Psychologist)
                .Include(p => p.Patient)
                .FirstOrDefault(p =>
                    p.PsychologistId == psychologistId &&
                    p.PatientId == patientId &&
                    p.Date == date);
        }

        public void UpdateAppointment(Planificator appointment)
        {
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));
            dbContext.Planificators.Update(appointment);
            dbContext.SaveChanges();
        }

        public void DeleteAppointment(int psychologistId, int patientId, DateTime date)
        {
            var appointment = GetAppointmentById(psychologistId, patientId, date);
            if (appointment != null)
            {
                dbContext.Planificators.Remove(appointment);
                dbContext.SaveChanges();
            }
        }
    }
}