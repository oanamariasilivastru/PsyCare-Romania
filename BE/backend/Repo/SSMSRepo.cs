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
    private readonly byte[] masterKey;

    public SSMSRepo(PSYCareDbContext dbContext, IConfiguration configuration)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        //this.connectionString = "Server=DESKTOP-29QIGKD;Database=PSYCare;Trusted_Connection=True;Encrypt=False;"; //configuration.GetConnectionString("PSYCare");

        string? base64Key = configuration["Vault:MasterKey"];
        if (string.IsNullOrWhiteSpace(base64Key))
            throw new ArgumentException("Vault master key missing in configuration.");

        this.masterKey = Convert.FromBase64String(base64Key);
    }
    private byte[] EncryptString(string plainText, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs, Encoding.UTF8))
                {
                    sw.Write(plainText);
                }

                return ms.ToArray();
            }
        }
    }
    
    public void addPatient(Patient p, string rawPnc)
    {
        if (p == null) throw new ArgumentNullException(nameof(p));
        if (string.IsNullOrWhiteSpace(rawPnc)) throw new ArgumentNullException(nameof(rawPnc));
        Guid token = Guid.NewGuid();
        byte[] encrypted = EncryptString(rawPnc, masterKey);
        
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
        throw new NotImplementedException();
    }
    
}