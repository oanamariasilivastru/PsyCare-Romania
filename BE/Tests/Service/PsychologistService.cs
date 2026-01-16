using backend.Domain;
using backend.Repo.Interfaces;
using backend.Service;
using backend.Utils;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

public class PsychologistServiceTests
{
    private IConfiguration GetTestConfiguration()
    {
        var settings = new Dictionary<string, string>
        {
            // JWT key suficient de lungă pentru HS256
            { "Jwt:Key", "ThisIsA32CharLongSecretKeyForJWT!" },
            { "Jwt:Issuer", "TestIssuer" },

            // Vault master key dummy (Base64 valid)
            { "Vault:MasterKey", "VGhpcyBpcyBhIGZha2UgbWFzdGVyIGtleQ==" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void AddPsychologist_ShouldCallRepo()
    {
        var mockRepo = new Mock<IPsychologistRepository>();
        var config = GetTestConfiguration();

        var service = new PsychologistService(mockRepo.Object, config);

        var psych = new Psychologist { Id = 1, Name = "Dr. Popescu" };
        string code = "ABC123";

        service.AddPsychologist(psych, code);

        mockRepo.Verify(x => x.AddPsychologist(psych, code), Times.Once);
    }

    [Fact]
    public void GetPsychologist_ShouldReturnPsychologist()
    {
        var mockRepo = new Mock<IPsychologistRepository>();
        var config = GetTestConfiguration();

        var psych = new Psychologist { Id = 1, Name = "Dr. Popescu" };
        mockRepo.Setup(x => x.GetPsychologist("Dr. Popescu")).Returns(psych);

        var service = new PsychologistService(mockRepo.Object, config);

        var result = service.GetPsychologist("Dr. Popescu");

        Assert.NotNull(result);
        Assert.Equal("Dr. Popescu", result.Name);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var config = GetTestConfiguration();
        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("mypassword", out salt);

        var psych = new Psychologist
        {
            Id = 1,
            Name = "Dr. Popescu",
            Password = hash,
            Salt = salt
        };

        var mockRepo = new Mock<IPsychologistRepository>();
        mockRepo.Setup(r => r.VerifyPassword("mypassword", hash, salt)).Returns(true);

        var service = new PsychologistService(mockRepo.Object, config);

        var result = true;

        Assert.True(result);
    }

    [Fact]
    public void LoginPsychologist_ShouldReturnJwt_WhenPasswordValid()
    {
        var mockRepo = new Mock<IPsychologistRepository>();
        var config = GetTestConfiguration();

        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("mypassword", out salt);

        var psych = new Psychologist
        {
            Id = 1,
            Name = "Dr. Popescu",
            Password = hash,
            Salt = salt
        };

        mockRepo.Setup(r => r.GetPsychologist("Dr. Popescu")).Returns(psych);
        mockRepo.Setup(r => r.VerifyPassword("mypassword", hash, salt)).Returns(true);

        var service = new PsychologistService(mockRepo.Object, config);

        var token = service.LoginPsychologist("Dr. Popescu", "mypassword");

        Assert.NotNull(token);
        Assert.Contains("eyJ", token); // simplu check că e JWT
    }
}
