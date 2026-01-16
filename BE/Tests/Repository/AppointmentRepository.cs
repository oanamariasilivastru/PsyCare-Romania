using backend.Domain;
using backend.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

public class AppointmentRepositoryTests
{
    private PSYCareDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    [Fact]
    public void AddAppointment_ShouldAddAppointment()
    {
        var context = GetInMemoryDbContext();
        var repo = new AppointmentRepository(context);

        var patient = new Patient
        {
            Id = 1,
            Name = "Ana",
            Password = "dummy",
            Salt = "dummy",
            IdentifierToken = Guid.NewGuid()  // obligatoriu
        };

        var psychologist = new Psychologist
        {
            Id = 10,
            Name = "Dr. X",
            Password = "dummy",
            Salt = "dummy",
            IdentifierToken = Guid.NewGuid()  // obligatoriu
        };

        context.Patients.Add(patient);
        context.Psychologists.Add(psychologist);
        context.SaveChanges();

        var appointment = new Planificator
        {
            PatientId = patient.Id,
            Patient = patient,
            PsychologistId = psychologist.Id,
            Psychologist = psychologist,
            Date = DateTime.UtcNow,
            Fee = 100
        };

        repo.AddAppointment(appointment);

        var saved = context.Planificators.FirstOrDefault();
        Assert.NotNull(saved);
        Assert.Equal(patient.Id, saved.PatientId);
        Assert.Equal(psychologist.Id, saved.PsychologistId);
        Assert.Equal(100, saved.Fee);
    }

    [Fact]
    public void GetPatientAppointments_ShouldReturnOnlyPatientAppointmentsOrdered()
    {
        var context = GetInMemoryDbContext();
        var repo = new AppointmentRepository(context);

        var patient1 = new Patient { Id = 1, Name = "Ana", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };
        var patient2 = new Patient { Id = 2, Name = "Ion", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };
        var psych = new Psychologist { Id = 10, Name = "Dr. X", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };

        context.Patients.AddRange(patient1, patient2);
        context.Psychologists.Add(psych);

        context.Planificators.AddRange(
            new Planificator { PatientId = 1, Patient = patient1, PsychologistId = 10, Psychologist = psych, Date = new DateTime(2026, 1, 20), Fee = 100 },
            new Planificator { PatientId = 1, Patient = patient1, PsychologistId = 10, Psychologist = psych, Date = new DateTime(2026, 1, 10), Fee = 120 },
            new Planificator { PatientId = 2, Patient = patient2, PsychologistId = 10, Psychologist = psych, Date = new DateTime(2026, 1, 15), Fee = 90 }
        );
        context.SaveChanges();

        var result = repo.GetPatientAppointments(1);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date < result[1].Date);
        Assert.All(result, a => Assert.Equal(1, a.PatientId));
    }

    [Fact]
    public void GetPsychologistAppointments_ShouldReturnOnlyPsychologistAppointmentsOrdered()
    {
        var context = GetInMemoryDbContext();
        var repo = new AppointmentRepository(context);

        var patient1 = new Patient { Id = 1, Name = "Ana", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };
        var patient2 = new Patient { Id = 2, Name = "Ion", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };
        var psych1 = new Psychologist { Id = 10, Name = "Dr. X", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };
        var psych2 = new Psychologist { Id = 11, Name = "Dr. Y", Password = "dummy", Salt = "dummy", IdentifierToken = Guid.NewGuid() };

        context.Patients.AddRange(patient1, patient2);
        context.Psychologists.AddRange(psych1, psych2);

        context.Planificators.AddRange(
            new Planificator { PatientId = 1, Patient = patient1, PsychologistId = 10, Psychologist = psych1, Date = new DateTime(2026, 1, 20), Fee = 100 },
            new Planificator { PatientId = 2, Patient = patient2, PsychologistId = 10, Psychologist = psych1, Date = new DateTime(2026, 1, 10), Fee = 120 },
            new Planificator { PatientId = 1, Patient = patient1, PsychologistId = 11, Psychologist = psych2, Date = new DateTime(2026, 1, 15), Fee = 90 }
        );
        context.SaveChanges();

        var result = repo.GetPsychologistAppointments(10);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date < result[1].Date);
        Assert.All(result, a => Assert.Equal(10, a.PsychologistId));
    }
}
