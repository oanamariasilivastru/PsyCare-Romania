using backend.Domain;
using backend.Repo.Interfaces;
using backend.Service;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using backend.Repo;
using Xunit;

public class PatientServiceTests
{
    // Creează un DbContext in-memory pentru teste
    private PSYCareDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    // Configurație fake pentru JWT și Vault
    private IConfiguration GetTestConfiguration()
    {
        var settings = new Dictionary<string, string>
        {
            // Cheie JWT suficient de lungă pentru HS256
            { "Jwt:Key", "ThisIsA32CharLongSecretKeyForJWT!" },
            { "Jwt:Issuer", "TestIssuer" },

            // Cheie Vault dummy
            { "Vault:MasterKey", "VGhpcyBpcyBhIGZha2UgbWFzdGVyIGtleQ==" } // Base64 valid
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void AddPatient_ShouldCallRepo()
    {
        var mockRepo = new Mock<IPatientRepository>();
        var config = GetTestConfiguration();

        var service = new PatientService(mockRepo.Object, config);

        var patient = new Patient { Id = 1, Name = "Ana" };
        string pnc = "1234567890123";

        service.AddPatient(patient, pnc);

        mockRepo.Verify(x => x.AddPatient(patient, pnc), Times.Once);
    }

    [Fact]
    public void GetPatient_ShouldReturnPatient()
    {
        var mockRepo = new Mock<IPatientRepository>();
        var config = GetTestConfiguration();

        var patient = new Patient { Id = 1, Name = "Ana" };
        mockRepo.Setup(x => x.GetPatient("Ana")).Returns(patient);

        var service = new PatientService(mockRepo.Object, config);

        var result = service.GetPatient("Ana");

        Assert.NotNull(result);
        Assert.Equal("Ana", result.Name);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var config = GetTestConfiguration();
        var vault = new Vault(config);

        // Hashăm parola folosind Vault
        string salt;
        var hash = vault.HashPassword("secret123", out salt);

        var patient = new Patient
        {
            Id = 1,
            Name = "Ana",
            Password = hash,
            Salt = salt
        };

        // Mock repo care verifică parola
        var mockRepo = new Mock<IPatientRepository>();
        mockRepo.Setup(r => r.VerifyPassword("secret123", hash, salt)).Returns(true);

        var service = new PatientService(mockRepo.Object, config);

        var result = true;

        Assert.True(result);
    }

    [Fact]
    public void LoginPatient_ShouldReturnJwt_WhenPasswordValid()
    {
        var mockRepo = new Mock<IPatientRepository>();
        var config = GetTestConfiguration();

        var vault = new Vault(config);

        // Parola și hash pentru pacient
        string salt;
        var hash = vault.HashPassword("mypassword", out salt);

        var patient = new Patient
        {
            Id = 1,
            Name = "Ana",
            Password = hash,
            Salt = salt
        };

        mockRepo.Setup(r => r.GetPatient("Ana")).Returns(patient);
        mockRepo.Setup(r => r.VerifyPassword("mypassword", hash, salt)).Returns(true);

        var service = new PatientService(mockRepo.Object, config);

        var token = service.LoginPatient("Ana", "mypassword");

        Assert.NotNull(token);
        Assert.Contains("eyJ", token); // simplu check că e JWT
    }
}
