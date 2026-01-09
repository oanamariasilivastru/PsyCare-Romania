using backend.Utils;

namespace backend.Repo;

using backend.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

public class SSMSRepo : IRepo
{
    private readonly PSYCareDbContext dbContext;
    private readonly string connectionString="Server=DESKTOP-29QIGKD;Database=PSYCare;Trusted_Connection=True;Encrypt=False;";
    private readonly Vault vault;
    public SSMSRepo(PSYCareDbContext dbContext, IConfiguration configuration)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        //this.connectionString = "Server=DESKTOP-29QIGKD;Database=PSYCare;Trusted_Connection=True;Encrypt=False;"; //configuration.GetConnectionString("PSYCare");
        this.vault = new Vault(configuration);
    }
    
   
    private static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash) == storedHash;
    }
    
    public void addPatient(Patient p, string rawPnc)
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

    public void addPsychologist(Psychologist p, string code)
    {
        if (p == null) throw new ArgumentNullException(nameof(p));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
        Guid token = Guid.NewGuid();
        byte[] encrypted = this.vault.EncryptString(code);
        string salt;
        p.Password = this.vault.HashPassword(p.Password, out salt);
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
    
    public Patient? getPatient(string name)
    {
        return dbContext.Patients
            .FirstOrDefault(p => p.Name == name);
    }
    
    public Psychologist? getPsychologist(string name)
    {
        return dbContext.Psychologists
            .FirstOrDefault(p => p.Name == name);
    }

    public string getPatientPNC(Patient p)
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
    
    public string getPsychologistStamp(Psychologist p)
    {
        byte[] encrypted;
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using var cmd = new SqlCommand(@"SELECT encrypted_value FROM VaultIdentifier WHERE token = @token", conn);
            cmd.Parameters.AddWithValue("@token", p.IdentifierToken);

            var result = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Token not found");

            encrypted = (byte[])result;
        }
        return vault.Decrypt(encrypted);
    }
    
}