using backend.Domain;
using backend.Repo;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using System;
using System.Collections.Generic;

public class PatientRepositoryTests
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
    public void GetPatient_ShouldReturnPatientByName()
    {
        var context = GetDbContext();
        context.Patients.Add(new Patient { Id = 1, Name = "Ana" });
        context.SaveChanges();

        var repo = new PatientRepository(context, GetConfiguration());

        var patient = repo.GetPatient("Ana");

        Assert.NotNull(patient);
        Assert.Equal(1, patient!.Id);
    }

    [Fact]
    public void GetPatientById_ShouldReturnCorrectPatient()
    {
        var context = GetDbContext();
        context.Patients.Add(new Patient { Id = 2, Name = "Maria" });
        context.SaveChanges();

        var repo = new PatientRepository(context, GetConfiguration());

        var patient = repo.GetPatientById(2);

        Assert.NotNull(patient);
        Assert.Equal("Maria", patient!.Name);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var config = GetConfiguration();
        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("secret123", out salt);

        var repo = new PatientRepository(GetDbContext(), config);

        var result = repo.VerifyPassword("secret123", hash, salt);

        Assert.True(result);
    }
}
