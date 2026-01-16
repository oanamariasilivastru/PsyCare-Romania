using backend.Domain;
using backend.Repo;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using System;
using System.Collections.Generic;

public class PsychologistRepositoryTests
{
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var settings = new Dictionary<string, string>
        {
            { "ConnectionStrings:PSYCare", "FakeConnectionString" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void GetPsychologist_ShouldReturnPsychologistByName()
    {
        var context = GetDbContext();
        context.Psychologists.Add(new Psychologist { Id = 1, Name = "Dr. Popescu" });
        context.SaveChanges();

        var repo = new PsychologistRepository(context, GetConfiguration());

        var psychologist = repo.GetPsychologist("Dr. Popescu");

        Assert.NotNull(psychologist);
        Assert.Equal(1, psychologist!.Id);
    }

    [Fact]
    public void GetPsychologistById_ShouldReturnCorrectPsychologist()
    {
        var context = GetDbContext();
        context.Psychologists.Add(new Psychologist { Id = 2, Name = "Dr. Ionescu" });
        context.SaveChanges();

        var repo = new PsychologistRepository(context, GetConfiguration());

        var psychologist = repo.GetPsychologistById(2);

        Assert.NotNull(psychologist);
        Assert.Equal("Dr. Ionescu", psychologist!.Name);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var config = GetConfiguration();
        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("securePass", out salt);

        var repo = new PsychologistRepository(GetDbContext(), config);

        var result = repo.VerifyPassword("securePass", hash, salt);

        Assert.True(result);
    }
}
