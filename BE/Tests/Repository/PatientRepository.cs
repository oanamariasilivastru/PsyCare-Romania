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
    // Creează un DbContext InMemory
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    // Creează un IConfiguration fake pentru Vault
    private IConfiguration GetConfiguration()
    {
        var settings = new Dictionary<string, string>
        {
            { "Vault:MasterKey", Convert.ToBase64String(Guid.NewGuid().ToByteArray()) },
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

        // Creează pacientul cu toate câmpurile obligatorii
        var patient = new Patient
        {
            Id = 1,
            Name = "Ana",
            Password = "dummy_password",
            Salt = "dummy_salt",
            IdentifierToken = Guid.NewGuid()
        };

        context.Patients.Add(patient);
        context.SaveChanges();

        var repo = new PatientRepository(context, GetConfiguration());

        var result = repo.GetPatient("Ana");

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Ana", result.Name);
    }

    [Fact]
    public void GetPatientById_ShouldReturnCorrectPatient()
    {
        var context = GetDbContext();

        var patient = new Patient
        {
            Id = 2,
            Name = "Maria",
            Password = "dummy_password",
            Salt = "dummy_salt",
            IdentifierToken = Guid.NewGuid()
        };

        context.Patients.Add(patient);
        context.SaveChanges();

        var repo = new PatientRepository(context, GetConfiguration());

        var result = repo.GetPatientById(2);

        Assert.NotNull(result);
        Assert.Equal("Maria", result!.Name);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var config = GetConfiguration();
        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("secret123", out salt);

        var patient = new Patient
        {
            Id = 1,
            Name = "Ana",
            Password = hash,
            Salt = salt,
            IdentifierToken = Guid.NewGuid()
        };

        var context = GetDbContext();
        context.Patients.Add(patient);
        context.SaveChanges();

        var repo = new PatientRepository(context, config);

        var result = repo.VerifyPassword("secret123", hash, salt);

        Assert.True(result);
    }
}
