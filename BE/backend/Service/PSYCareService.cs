using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Domain;
using backend.Repo;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Service
{
    public class PSYCareService : IService
    {
        private readonly IRepo repo;
        private readonly IConfiguration _configuration;

        public PSYCareService(IRepo repo, IConfiguration configuration)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddPatient(Patient patient, string pnc)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (string.IsNullOrWhiteSpace(pnc)) throw new ArgumentException("PNC is required");
            repo.addPatient(patient, pnc);
        }

        public void AddPsychologist(Psychologist psychologist, string code)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required");
            repo.addPsychologist(psychologist, code);
        }

        public Patient? GetPatient(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return repo.getPatient(name);
        }

        public Psychologist? GetPsychologist(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return repo.getPsychologist(name);
        }

        public string GetPatientPNC(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            return repo.getPatientPNC(patient);
        }

        public string GetPsychologistStamp(Psychologist psychologist)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            return repo.getPsychologistStamp(psychologist);
        }

        public string? LoginPatient(string name, string password)
        {
            var patient = repo.getPatient(name);
            if (patient == null) return null;

            if (!SSMSRepo.VerifyPassword(password, patient.Password, patient.Salt))
                return null;

            return GenerateJwtToken(patient.Name, "Patient");
        }

        public string? LoginPsychologist(string name, string password)
        {
            var psych = repo.getPsychologist(name);
            if (psych == null) return null;

            if (!SSMSRepo.VerifyPassword(password, psych.Password, psych.Salt))
                return null;

            return GenerateJwtToken(psych.Name, "Psychologist");
        }

        private string GenerateJwtToken(string userName, string role)
        {
            var key = _configuration["Jwt:Key"] ?? "SuperSecretKey12345";
            var issuer = _configuration["Jwt:Issuer"] ?? "PSYCare";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
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
