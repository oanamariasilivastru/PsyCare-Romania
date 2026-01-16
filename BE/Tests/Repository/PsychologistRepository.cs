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
            // MasterKey fake, dar valid Base64
            { "Vault:MasterKey", Convert.ToBase64String(Guid.NewGuid().ToByteArray()) }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void GetPsychologist_ShouldReturnByName()
    {
        var context = GetDbContext();
        var vault = new Vault(GetConfiguration());

        string salt;
        var hash = vault.HashPassword("pass123", out salt);

        var psych = new Psychologist
        {
            Id = 10,
            Name = "Dr. X",
            Password = hash,
            Salt = salt,
            IdentifierToken = Guid.NewGuid()
        };

        context.Psychologists.Add(psych);
        context.SaveChanges();

        var repo = new PsychologistRepository(context, GetConfiguration());

        var result = repo.GetPsychologist("Dr. X");

        Assert.NotNull(result);
        Assert.Equal(10, result!.Id);
        Assert.Equal("Dr. X", result.Name);
    }

    [Fact]
    public void VerifyPsychologistPassword_ShouldReturnTrue()
    {
        var config = GetConfiguration();
        var vault = new Vault(config);

        string salt;
        var hash = vault.HashPassword("pass123", out salt);

        var psych = new Psychologist
        {
            Id = 11,
            Name = "Dr. Y",
            Password = hash,
            Salt = salt,
            IdentifierToken = Guid.NewGuid()
        };

        var context = GetDbContext();
        context.Psychologists.Add(psych);
        context.SaveChanges();

        var repo = new PsychologistRepository(context, config);

        var result = repo.VerifyPassword("pass123", hash, salt);

        Assert.True(result);
    }
}
