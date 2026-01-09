using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        // Patient? LoginPatient(string name, string password);
        // Psychologist? LoginPsychologist(string name, string password);
    }
}
