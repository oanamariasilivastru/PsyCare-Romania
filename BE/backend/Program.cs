using backend.Domain;
using backend.Repo;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace backend
{
    class Program
    {
        static void Main()
        {
            // try
            // {
            //     Console.WriteLine("=== PSYCare Application Started ===\n");
            //
            //     var builder = new ConfigurationBuilder()
            //         .AddJsonFile(@"C:\Master\An_1\req_eng\PsyCare-Romania\BE\backend\appsettings.json",
            //                      optional: false,
            //                      reloadOnChange: true);
            //
            //
            //     IConfiguration configuration = builder.Build();
            //
            //     string masterKey = configuration["Vault:MasterKey"];
            //     if (string.IsNullOrEmpty(masterKey))
            //     {
            //         Console.WriteLine("ERROR: Vault MasterKey not found in configuration!");
            //         return;
            //     }
            //
            //     Console.WriteLine("✓ Vault MasterKey loaded successfully.\n");

            //     // --- WORKING WITH PATIENT ---
            //     Console.WriteLine("--- PATIENT OPERATIONS ---");
            //     using (var dbContext = new PSYCareDbContext())
            //     {
            //         try
            //         {
            //             // Test database connection
            //             if (!dbContext.Database.CanConnect())
            //             {
            //                 Console.WriteLine("ERROR: Cannot connect to database!");
            //                 Console.WriteLine("Please ensure SQL Server is running and connection string is correct.");
            //                 return;
            //             }
            //             Console.WriteLine("✓ Database connection successful.");
            //
            //             var repo = new SSMSRepo(dbContext, configuration);
            //
            //             string patientName = "John Doe";
            //             string patientPassword = "password123";
            //             string patientPnc = "1234567890123";
            //
            //             // Try to get patient
            //             Console.WriteLine($"Searching for patient: {patientName}");
            //             var patient = repo.getPatient(patientName);
            //
            //             // If not found, create patient
            //             if (patient == null)
            //             {
            //                 Console.WriteLine($"Patient '{patientName}' not found. Creating new patient...");
            //
            //                 patient = new Patient
            //                 {
            //                     Name = patientName,
            //                     Password = patientPassword
            //                 };
            //
            //                 repo.addPatient(patient, patientPnc);
            //                 Console.WriteLine($"✓ Successfully added new patient: {patient.Name}");
            //
            //                 // Refresh patient from database to get updated data
            //                 patient = repo.getPatient(patientName);
            //             }
            //             else
            //             {
            //                 Console.WriteLine($"✓ Patient already exists: {patient.Name}");
            //             }
            //
            //             // Fetch PNC securely
            //             if (patient != null && patient.IdentifierToken.HasValue && patient.IdentifierToken.Value != Guid.Empty)
            //             {
            //                 try
            //                 {
            //                     var pnc = repo.getPatientPNC(patient);
            //                     Console.WriteLine($"✓ Patient PNC retrieved: {pnc}");
            //                 }
            //                 catch (Exception ex)
            //                 {
            //                     Console.WriteLine($"✗ Error retrieving PNC: {ex.Message}");
            //                 }
            //             }
            //             else
            //             {
            //                 Console.WriteLine("✗ Patient has no IdentifierToken.");
            //             }
            //         }
            //         catch (Exception ex)
            //         {
            //             Console.WriteLine($"ERROR in Patient operations: {ex.Message}");
            //             if (ex.InnerException != null)
            //             {
            //                 Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            //             }
            //         }
            //     }
            //
            //     Console.WriteLine();
            //
            //     // --- WORKING WITH PSYCHOLOGIST ---
            //     Console.WriteLine("--- PSYCHOLOGIST OPERATIONS ---");
            //     using (var dbContext = new PSYCareDbContext())
            //     {
            //         try
            //         {
            //             var repo = new SSMSRepo(dbContext, configuration);
            //
            //             string psychName = "Dr. Smith";
            //             string psychPassword = "psych123";
            //             string psychCode = "176432";
            //
            //             // Try to get psychologist
            //             Console.WriteLine($"Searching for psychologist: {psychName}");
            //             var psych = repo.getPsychologist(psychName);
            //
            //             // If not found, create psychologist
            //             if (psych == null)
            //             {
            //                 Console.WriteLine($"Psychologist '{psychName}' not found. Creating new psychologist...");
            //
            //                 psych = new Psychologist(psychName, psychPassword, "aa" +
            //                                                                    "");
            //
            //                 repo.addPsychologist(psych, psychCode);
            //                 Console.WriteLine($"✓ Successfully added new psychologist: {psych.Name}");
            //
            //                 // Refresh psychologist from database
            //                 psych = repo.getPsychologist(psychName);
            //             }
            //             else
            //             {
            //                 Console.WriteLine($"✓ Psychologist already exists: {psych.Name}");
            //             }
            //
            //             // Fetch Stamp Code securely
            //             if (psych != null && psych.IdentifierToken.HasValue && psych.IdentifierToken.Value != Guid.Empty)
            //             {
            //                 try
            //                 {
            //                     var code = repo.getPsychologistStamp(psych);
            //                     Console.WriteLine($"✓ Psychologist Stamp Code retrieved: {code}");
            //                 }
            //                 catch (Exception ex)
            //                 {
            //                     Console.WriteLine($"✗ Error retrieving Stamp Code: {ex.Message}");
            //                 }
            //             }
            //             else
            //             {
            //                 Console.WriteLine("✗ Psychologist has no IdentifierToken.");
            //             }
            //         }
            //         catch (Exception ex)
            //         {
            //             Console.WriteLine($"ERROR in Psychologist operations: {ex.Message}");
            //             if (ex.InnerException != null)
            //             {
            //                 Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            //             }
            //         }
            //     }
            //
            //     Console.WriteLine("\n=== PSYCare Application Finished Successfully ===");
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"\n=== CRITICAL ERROR ===");
            //     Console.WriteLine($"Message: {ex.Message}");
            //     Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");
            //     if (ex.InnerException != null)
            //     {
            //         Console.WriteLine($"\nInner Exception: {ex.InnerException.Message}");
            //     }
            // }
            // finally
            // {
            //     Console.WriteLine("\nPress any key to exit...");
            //     Console.ReadKey();
            // }
        }
    }
}