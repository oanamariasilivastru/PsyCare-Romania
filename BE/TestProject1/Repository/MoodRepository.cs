using backend.Domain;
using backend.Repo;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

public class MoodRepositoryTests
{
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    [Fact]
    public void AddMood_ShouldPersistMood()
    {
        var context = GetDbContext();
        var repo = new MoodRepository(context);

        var mood = new Mood
        {
            PatientId = 1,
            Date = DateTime.Today,
            Score = 4
        };

        repo.AddMood(mood);

        Assert.Single(context.Moods);
    }

    [Fact]
    public void GetMoods_ShouldReturnOnlyPatientMoodsOrdered()
    {
        var context = GetDbContext();
        context.Moods.AddRange(
            new Mood { PatientId = 1, Date = new DateTime(2025, 1, 2), Score = 3 },
            new Mood { PatientId = 1, Date = new DateTime(2025, 1, 1), Score = 5 },
            new Mood { PatientId = 2, Date = new DateTime(2025, 1, 1), Score = 2 }
        );
        context.SaveChanges();

        var repo = new MoodRepository(context);

        var result = repo.GetMoods(1);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date < result[1].Date);
    }

    [Fact]
    public void GetMoodById_ShouldReturnCorrectMood()
    {
        var context = GetDbContext();
        var date = new DateTime(2025, 1, 1);

        context.Moods.Add(new Mood
        {
            PatientId = 1,
            Date = date,
            Score = 5
        });
        context.SaveChanges();

        var repo = new MoodRepository(context);

        var mood = repo.GetMoodById(1, date);

        Assert.NotNull(mood);
        Assert.Equal(5, mood!.Score);
    }
}