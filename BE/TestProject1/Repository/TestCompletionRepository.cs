using backend.Domain;
using backend.Repo;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using PSYCare.Repositories;

public class TestCompletionRepositoryTests
{
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    [Fact]
    public async Task CreateTestCompletionAsync_ShouldPersistTestCompletion()
    {
        var context = GetDbContext();
        var repo = new TestCompletionRepository(context);

        var testCompletion = new TestCompletion
        {
            PatientId = 1,
            TestCode = "phq9",
            TotalScore = 12,
            Severity = "Moderate",
            CompletedAt = DateTime.UtcNow
        };

        await repo.CreateTestCompletionAsync(testCompletion);

        Assert.Single(context.TestCompletions);
    }

    [Fact]
    public async Task GetPatientTestCompletionsAsync_ShouldReturnOrderedCompletions()
    {
        var context = GetDbContext();
        context.TestCompletions.AddRange(
            new TestCompletion
            {
                PatientId = 1,
                TestCode = "phq9",
                CompletedAt = new DateTime(2025, 1, 1)
            },
            new TestCompletion
            {
                PatientId = 1,
                TestCode = "gad7",
                CompletedAt = new DateTime(2025, 1, 3)
            },
            new TestCompletion
            {
                PatientId = 2,
                TestCode = "phq9",
                CompletedAt = new DateTime(2025, 1, 2)
            }
        );
        await context.SaveChangesAsync();

        var repo = new TestCompletionRepository(context);

        var result = await repo.GetPatientTestCompletionsAsync(1);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].CompletedAt > result[1].CompletedAt);
    }

    [Fact]
    public async Task DeleteTestCompletionAsync_ShouldRemoveTestCompletion()
    {
        var context = GetDbContext();
        var testCompletion = new TestCompletion
        {
            PatientId = 1,
            TestCode = "phq9",
            CompletedAt = DateTime.UtcNow
        };

        context.TestCompletions.Add(testCompletion);
        await context.SaveChangesAsync();

        var repo = new TestCompletionRepository(context);

        var result = await repo.DeleteTestCompletionAsync(testCompletion.Id);

        Assert.True(result);
        Assert.Empty(context.TestCompletions);
    }
}
