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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository patientRepo;
        private readonly IConfiguration configuration;

        public PatientService(IPatientRepository patientRepo, IConfiguration configuration)
        {
            this.patientRepo = patientRepo ?? throw new ArgumentNullException(nameof(patientRepo));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddPatient(Patient patient, string pnc)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (string.IsNullOrWhiteSpace(pnc)) throw new ArgumentException("PNC is required");
            patientRepo.AddPatient(patient, pnc);
        }

        public Patient? GetPatient(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return patientRepo.GetPatient(name);
        }

        public Patient? GetPatientById(int id)
        {
            return patientRepo.GetPatientById(id);
        }

        public string GetPatientPNC(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            return patientRepo.GetPatientPNC(patient);
        }

        public string? LoginPatient(string name, string password)
        {
            var patient = patientRepo.GetPatient(name);
            if (patient == null) return null;

            if (!patientRepo.VerifyPassword(password, patient.Password, patient.Salt))
                return null;

            return GenerateJwtToken(patient.Id, patient.Name);
        }

        private string GenerateJwtToken(int patientId, string userName)
        {
            var key = configuration["Jwt:Key"] ?? "SuperSecretKey12345";
            var issuer = configuration["Jwt:Issuer"] ?? "PSYCare";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, patientId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("patientId", patientId.ToString())
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