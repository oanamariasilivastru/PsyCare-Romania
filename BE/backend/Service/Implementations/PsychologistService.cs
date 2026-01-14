using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Domain;
using backend.Repo;
using backend.Repo.Interfaces;
using backend.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Service
{
    public class PsychologistService : IPsychologistService
    {
        private readonly IPsychologistRepository psychologistRepo;
        private readonly IConfiguration configuration;

        public PsychologistService(IPsychologistRepository psychologistRepo, IConfiguration configuration)
        {
            this.psychologistRepo = psychologistRepo ?? throw new ArgumentNullException(nameof(psychologistRepo));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddPsychologist(Psychologist psychologist, string code)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required");
            psychologistRepo.AddPsychologist(psychologist, code);
        }

        public Psychologist? GetPsychologist(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return psychologistRepo.GetPsychologist(name);
        }

        public Psychologist? GetPsychologistById(int id)
        {
            return psychologistRepo.GetPsychologistById(id);
        }

        public string GetPsychologistStamp(Psychologist psychologist)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            return psychologistRepo.GetPsychologistStamp(psychologist);
        }

        public string? LoginPsychologist(string name, string password)
        {
            var psych = psychologistRepo.GetPsychologist(name);
            if (psych == null) return null;

            if (!psychologistRepo.VerifyPassword(password, psych.Password, psych.Salt))
                return null;

            return GenerateJwtToken(psych.Id, psych.Name);
        }

        private string GenerateJwtToken(int psychologistId, string userName)
        {
            var key = configuration["Jwt:Key"] ?? "SuperSecretKey12345";
            var issuer = configuration["Jwt:Issuer"] ?? "PSYCare";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, psychologistId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Psychologist"),
                new Claim("psychologistId", psychologistId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}