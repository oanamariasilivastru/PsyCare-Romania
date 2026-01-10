using System;
using backend.Domain;

namespace backend.Service
{
    public interface IService
    {
        void AddPatient(Patient patient, string pnc);
        void AddPsychologist(Psychologist psychologist, string code);
        
        Patient? GetPatient(string name);
        Psychologist? GetPsychologist(string name);
        
        string GetPatientPNC(Patient patient);
        string GetPsychologistStamp(Psychologist psychologist);
        
        string? LoginPatient(string name, string password);
        string? LoginPsychologist(string name, string password);
    }
}