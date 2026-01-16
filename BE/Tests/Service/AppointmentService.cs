using backend.Domain;
using backend.Dtos;
using backend.Repo.Interfaces;
using backend.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class AppointmentServiceTests
{
    [Fact]
    public void AddAppointment_ShouldCallRepo()
    {
        // Arrange
        var patient = new Patient { Id = 1, Name = "Ana" };
        var psychologist = new Psychologist { Id = 2, Name = "Dr. Popescu" };

        var mockPatientRepo = new Mock<IPatientRepository>();
        mockPatientRepo.Setup(x => x.GetPatientById(1)).Returns(patient);

        var mockPsychologistRepo = new Mock<IPsychologistRepository>();
        mockPsychologistRepo.Setup(x => x.GetPsychologistById(2)).Returns(psychologist);

        var mockAppointmentRepo = new Mock<IAppointmentRepository>();

        var service = new AppointmentService(mockAppointmentRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        var dto = new AppointmentDto
        {
            PatientId = 1,
            PsychologistId = 2,
            Date = DateTime.Today,
            Fee = 100
        };

        // Act
        service.AddAppointment(dto);

        // Assert
        mockAppointmentRepo.Verify(x => x.AddAppointment(It.Is<Planificator>(
            a => a.PatientId == 1 && a.PsychologistId == 2 && a.Fee == 100)), Times.Once);
    }

    [Fact]
    public void GetPatientAppointments_ShouldReturnMappedDtos()
    {
        // Arrange
        var patientId = 1;
        var appointments = new List<Planificator>
        {
            new Planificator
            {
                PatientId = patientId,
                Patient = new Patient { Id = patientId, Name = "Ana" },
                PsychologistId = 2,
                Psychologist = new Psychologist { Id = 2, Name = "Dr. Popescu" },
                Date = DateTime.Today,
                Fee = 120
            }
        };

        var mockAppointmentRepo = new Mock<IAppointmentRepository>();
        mockAppointmentRepo.Setup(x => x.GetPatientAppointments(patientId)).Returns(appointments);

        var mockPatientRepo = new Mock<IPatientRepository>();
        var mockPsychologistRepo = new Mock<IPsychologistRepository>();

        var service = new AppointmentService(mockAppointmentRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        // Act
        var result = service.GetPatientAppointments(patientId);

        // Assert
        Assert.Single(result);
        Assert.Equal("Ana", result[0].PatientName);
        Assert.Equal("Dr. Popescu", result[0].PsychologistName);
        Assert.Equal(120, result[0].Fee);
    }

    [Fact]
    public void GetPsychologistAppointments_ShouldReturnMappedDtos()
    {
        // Arrange
        var psychologistId = 2;
        var appointments = new List<Planificator>
        {
            new Planificator
            {
                PatientId = 1,
                Patient = new Patient { Id = 1, Name = "Ana" },
                PsychologistId = psychologistId,
                Psychologist = new Psychologist { Id = psychologistId, Name = "Dr. Popescu" },
                Date = DateTime.Today,
                Fee = 120
            }
        };

        var mockAppointmentRepo = new Mock<IAppointmentRepository>();
        mockAppointmentRepo.Setup(x => x.GetPsychologistAppointments(psychologistId)).Returns(appointments);

        var mockPatientRepo = new Mock<IPatientRepository>();
        var mockPsychologistRepo = new Mock<IPsychologistRepository>();

        var service = new AppointmentService(mockAppointmentRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        // Act
        var result = service.GetPsychologistAppointments(psychologistId);

        // Assert
        Assert.Single(result);
        Assert.Equal("Ana", result[0].PatientName);
        Assert.Equal("Dr. Popescu", result[0].PsychologistName);
        Assert.Equal(120, result[0].Fee);
    }
}
