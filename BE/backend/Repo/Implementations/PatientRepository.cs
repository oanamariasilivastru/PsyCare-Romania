using backend.Domain;
using backend.Repo.Interfaces;
using backend.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace backend.Repo
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PSYCareDbContext dbContext;
        private readonly string connectionString;
        private readonly Vault vault;

        public PatientRepository(PSYCareDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.connectionString = configuration.GetConnectionString("PSYCare");
            this.vault = new Vault(configuration);
        }

        public void AddPatient(Patient p, string rawPnc)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (string.IsNullOrWhiteSpace(rawPnc)) throw new ArgumentNullException(nameof(rawPnc));

            Guid token = Guid.NewGuid();
            byte[] encrypted = this.vault.EncryptString(rawPnc);
            string salt;
            p.Password = this.vault.HashPassword(p.Password, out salt);
            p.Salt = salt;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO VaultIdentifier 
                        (token, encrypted_value, data_type, retention_until) 
                    VALUES 
                        (@token, @encrypted, 'PNC', DATEADD(year, 5, GETDATE()))";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@encrypted", encrypted);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            p.IdentifierToken = token;
            dbContext.Patients.Add(p);
            dbContext.SaveChanges();
        }

        public Patient? GetPatient(string name)
        {
            return dbContext.Patients.FirstOrDefault(p => p.Name == name);
        }

        public Patient? GetPatientById(int id)
        {
            return dbContext.Patients.FirstOrDefault(p => p.Id == id);
        }

        public string GetPatientPNC(Patient p)
        {
            byte[] encrypted;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using var cmd = new SqlCommand(@"
                    SELECT encrypted_value 
                    FROM VaultIdentifier
                    WHERE token = @token", conn);
                cmd.Parameters.AddWithValue("@token", p.IdentifierToken);

                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception("Token not found");

                encrypted = (byte[])result;
            }
            return vault.Decrypt(encrypted);
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return this.vault.VerifyPassword(password, storedHash, storedSalt);
        }
    }
}