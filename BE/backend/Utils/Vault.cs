using System.Text;

namespace backend.Utils;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;


public class Vault
{
    private readonly byte[] masterKey;
    public Vault(IConfiguration configuration)
    {
        string? base64Key = configuration["Vault:MasterKey"];
        if (string.IsNullOrWhiteSpace(base64Key))
            throw new ArgumentException("Vault master key missing in configuration.");

        this.masterKey = Convert.FromBase64String(base64Key);
    }

    public string HashPassword(string password, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }
    
    public byte[] EncryptString(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = this.masterKey;
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
    
    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash) == storedHash;
    }
    
    public string Decrypt(byte[] encryptedData)
    {
        using var aes = Aes.Create();
        aes.Key = masterKey;
        
        byte[] iv = new byte[aes.BlockSize / 8];
        Array.Copy(encryptedData, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);
        return sr.ReadToEnd();
    }


}