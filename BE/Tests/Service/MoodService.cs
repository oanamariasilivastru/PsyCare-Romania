using backend.Domain;
using backend.Dtos;
using backend.Repo.Interfaces;
using backend.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class MoodServiceTests
{
    [Fact]
    public void AddMood_ShouldCallRepo()
    {
        // Arrange
        var patient = new Patient { Id = 1, Name = "Ana" };

        var mockMoodRepo = new Mock<IMoodRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();
        mockPatientRepo.Setup(x => x.GetPatientById(1)).Returns(patient);

        var service = new MoodService(mockMoodRepo.Object, mockPatientRepo.Object);

        var dto = new MoodDto
        {
            PatientId = 1,
            Date = DateTime.Today,
            Score = 8
        };

        // Act
        service.AddMood(dto);

        // Assert
        mockMoodRepo.Verify(x => x.AddMood(It.Is<Mood>(
            m => m.PatientId == 1 && m.Score == 8 && m.Date == DateTime.Today)), Times.Once);
    }

    [Fact]
    public void GetMoods_ShouldReturnMappedDtos()
    {
        // Arrange
        var patientId = 1;
        var moods = new List<Mood>
        {
            new Mood { PatientId = patientId, Date = new DateTime(2026,1,15), Score = 7 },
            new Mood { PatientId = patientId, Date = new DateTime(2026,1,14), Score = 9 }
        };

        var mockMoodRepo = new Mock<IMoodRepository>();
        mockMoodRepo.Setup(x => x.GetMoods(patientId)).Returns(moods);

        var mockPatientRepo = new Mock<IPatientRepository>();
        var service = new MoodService(mockMoodRepo.Object, mockPatientRepo.Object);

        // Act
        var result = service.GetMoods(patientId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Score == 7 && r.CompletionDate == new DateTime(2026,1,15));
        Assert.Contains(result, r => r.Score == 9 && r.CompletionDate == new DateTime(2026,1,14));
    }
}