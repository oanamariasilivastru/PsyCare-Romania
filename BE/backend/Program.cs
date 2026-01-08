using System.Security.Cryptography;

namespace backend;
using backend.Domain;
using backend.Repo;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        // byte[] key = new byte[32]; // 32 bytes = 256 bits
        // using (var rng = RandomNumberGenerator.Create())
        // {
        //     rng.GetBytes(key);
        // }
        // string base64Key = Convert.ToBase64String(key);
        // Console.WriteLine(base64Key);
        var builder = new ConfigurationBuilder()
            .AddJsonFile(@"D:\RE\backend\BE\backend\appsettings.json", optional: false, reloadOnChange: true);
        
        IConfiguration configuration = builder.Build();
        
        
        using (var dbContext = new PSYCareDbContext())
        {
            
            var repo = new SSMSRepo(dbContext, configuration);
        
            
            string patientName = "John Doe";
            string patientPassword = "password123";
            string patientSalt = "randomSalt123";
            string patientPnc = "1234567890123";
        
            var newPatient = new Patient(patientName, patientPassword, patientSalt);
        
            
            repo.addPatient(newPatient, patientPnc);
        
            Console.WriteLine($"Patient added: {newPatient.Name}, Token: {newPatient.IdentifierToken}");
        }
        
        Console.WriteLine("Done. Press any key to exit.");
        Console.ReadKey();
    }
}