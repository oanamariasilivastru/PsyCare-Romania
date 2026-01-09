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
        
            var p=repo.getPatient("John Doe");
            var pnc=repo.getPatientPNC(p);
        
            Console.WriteLine($"Patient added: {p.Name}, pnc: {pnc}");
        }
        
    using (var dbContext = new PSYCareDbContext())
    {
        
        var repo = new SSMSRepo(dbContext, configuration);
    
        
        string patientName = "John Doe";
        string patientPassword = "password123";
        string patientSalt = "randomSalt123";
    
        var psychologist = new Psychologist(patientName, patientPassword, patientSalt);
    
        
        //repo.addPsychologist(psychologist,"176432");
        var p=repo.getPsychologist("John Doe");
        var code=repo.getPsychologistStamp(p);
        Console.WriteLine($"Psychologist added: {psychologist.Name}, code: {code}");
    }
    }
}