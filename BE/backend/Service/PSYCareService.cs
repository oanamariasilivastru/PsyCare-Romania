using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain;
using backend.Repo;

namespace backend.Service
{
    public class PSYCareService : IService
    {
        private readonly IRepo repo;

        public PSYCareService(IRepo repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }


        public void AddPatient(Patient patient, string pnc)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (string.IsNullOrWhiteSpace(pnc)) throw new ArgumentException("PNC is required");
            repo.addPatient(patient, pnc);
        }

        public void AddPsychologist(Psychologist psychologist, string code)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required");
            repo.addPsychologist(psychologist, code);
        }


        public Patient? GetPatient(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return repo.getPatient(name);
        }

        public Psychologist? GetPsychologist(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return repo.getPsychologist(name);
        }


        public string GetPatientPNC(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            return repo.getPatientPNC(patient);
        }


        public string GetPsychologistStamp(Psychologist psychologist)
        {
            if (psychologist == null) throw new ArgumentNullException(nameof(psychologist));
            return repo.getPsychologistStamp(psychologist);
        }

        /*
        public Patient? LoginPatient(string name, string password)
        {
            var patient = repo.getPatient(name);
            if (patient == null) return null;

            return repo.VerifyPassword(patient, password) ? patient : null;
        }


        public Psychologist? LoginPsychologist(string name, string password)
        {
            var psychologist = repo.getPsychologist(name);
            if (psychologist == null) return null;

            return repo.VerifyPassword(psychologist, password) ? psychologist : null;
        }
        */
    }
}
