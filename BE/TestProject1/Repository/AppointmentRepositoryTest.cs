using backend.Domain;
using backend.Repo;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

public class AppointmentRepositoryTests
{
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    [Fact]
    public void AddAppointment_ShouldPersistAppointment()
    {
        var context = GetDbContext();
        var repo = new AppointmentRepository(context);

        var appointment = new Planificator
        {
            PatientId = 1,
            PsychologistId = 2,
            Date = DateTime.Now
        };

        repo.AddAppointment(appointment);

        Assert.Single(context.Planificators);
    }

    [Fact]
    public void GetPatientAppointments_ShouldReturnOnlyPatientAppointmentsOrdered()
    {
        var context = GetDbContext();
        context.Planificators.AddRange(
            new Planificator { PatientId = 1, PsychologistId = 1, Date = new DateTime(2025, 1, 2) },
            new Planificator { PatientId = 1, PsychologistId = 2, Date = new DateTime(2025, 1, 1) },
            new Planificator { PatientId = 2, PsychologistId = 1, Date = new DateTime(2025, 1, 3) }
        );
        context.SaveChanges();

        var repo = new AppointmentRepository(context);

        var result = repo.GetPatientAppointments(1);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date < result[1].Date);
    }

    [Fact]
    public void DeleteAppointment_ShouldRemoveAppointment()
    {
        var context = GetDbContext();
        var date = DateTime.Now;

        context.Planificators.Add(new Planificator
        {
            PatientId = 1,
            PsychologistId = 1,
            Date = date
        });
        context.SaveChanges();

        var repo = new AppointmentRepository(context);

        repo.DeleteAppointment(1, 1, date);

        Assert.Empty(context.Planificators);
    }
}
