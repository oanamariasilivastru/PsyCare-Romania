using backend.Domain;
using backend.Repo.Interfaces;
using backend.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace backend.Repo
{
    public class PsychologistRepository : IPsychologistRepository
    {
        private readonly PSYCareDbContext dbContext;
        private readonly string connectionString;
        private readonly Vault vault;

        public PsychologistRepository(PSYCareDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.connectionString = configuration.GetConnectionString("PSYCare");
            this.vault = new Vault(configuration);
        }

        public void AddPsychologist(Psychologist p, string code)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            Guid token = Guid.NewGuid();
            byte[] encrypted = this.vault.EncryptString(code);
            string salt;
            p.Password = this.vault.HashPassword(p.Password, out salt);
            p.Salt = salt;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO VaultIdentifier 
                        (token, encrypted_value, data_type, retention_until) 
                    VALUES 
                        (@token, @encrypted, 'STAMP_CODE', DATEADD(year, 20, GETDATE()))";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@encrypted", encrypted);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            p.IdentifierToken = token;
            dbContext.Psychologists.Add(p);
            dbContext.SaveChanges();
        }

        public Psychologist? GetPsychologist(string name)
        {
            return dbContext.Psychologists.FirstOrDefault(p => p.Name == name);
        }

        public Psychologist? GetPsychologistById(int id)
        {
            return dbContext.Psychologists.FirstOrDefault(p => p.Id == id);
        }

        public string GetPsychologistStamp(Psychologist p)
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