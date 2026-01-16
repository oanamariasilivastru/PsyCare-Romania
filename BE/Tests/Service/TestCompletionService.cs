using backend.Domain;
using backend.Dtos.Tests;
using backend.Repo.Interfaces;
using backend.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class TestCompletionServiceTests
{
    [Fact]
    public async Task CreateTestCompletion_ShouldCallRepo()
    {
        // Arrange
        var mockRepo = new Mock<ITestCompletionRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();

        mockPatientRepo.Setup(p => p.GetPatientById(1))
            .Returns(new Patient { Id = 1, Name = "Ana" });

        var service = new TestCompletionService(mockRepo.Object, mockPatientRepo.Object);

        var dto = new TestCompletionDto
        {
            PatientId = 1,
            TestCode = "PHQ9",
            TotalScore = 12,
            ResultLabel = "Moderate",
            Severity = "Moderate",
            Answers = new List<TestAnswerDto>
            {
                new TestAnswerDto { QuestionId = 1, SelectedScore = 1 }
            }
        };

        // Act
        await service.CreateTestCompletion(dto);

        // Assert
        mockRepo.Verify(r => r.CreateTestCompletionAsync(It.Is<TestCompletion>(
            t => t.PatientId == dto.PatientId &&
                 t.TestCode == dto.TestCode.ToLower() &&
                 t.TotalScore == dto.TotalScore &&
                 t.Answers.Contains("\"QuestionId\":1") // simplu check că Answers e serializat
        )), Times.Once);
    }


    [Fact]
    public async Task GetPatientTestCompletions_ShouldReturnMappedList()
    {
        // Arrange
        var mockRepo = new Mock<ITestCompletionRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();

        var completions = new List<TestCompletion>
        {
            new TestCompletion { Id = 1, PatientId = 1, TestCode = "phq9", TotalScore = 10, CompletedAt = DateTime.UtcNow }
        };

        mockRepo.Setup(r => r.GetPatientTestCompletionsAsync(1))
            .ReturnsAsync(completions);

        mockPatientRepo.Setup(p => p.GetPatientById(1))
            .Returns(new Patient { Id = 1, Name = "Ana" });

        var service = new TestCompletionService(mockRepo.Object, mockPatientRepo.Object);

        // Act
        var result = await service.GetPatientTestCompletions(1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Ana", result[0].PatientName);
        Assert.Equal("phq9", result[0].TestCode);
    }

    [Fact]
    public async Task DeleteTestCompletion_ShouldReturnRepoResult()
    {
        // Arrange
        var mockRepo = new Mock<ITestCompletionRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();

        mockRepo.Setup(r => r.DeleteTestCompletionAsync(1)).ReturnsAsync(true);

        var service = new TestCompletionService(mockRepo.Object, mockPatientRepo.Object);

        // Act
        var result = await service.DeleteTestCompletion(1);

        // Assert
        Assert.True(result);
        mockRepo.Verify(r => r.DeleteTestCompletionAsync(1), Times.Once);
    }
}
