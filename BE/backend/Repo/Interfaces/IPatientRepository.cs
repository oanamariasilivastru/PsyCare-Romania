using backend.Domain;

namespace backend.Repo.Interfaces;

public interface IPatientRepository
{
    void AddPatient(Patient patient, string rawPnc);
    Patient? GetPatient(string name);
    Patient? GetPatientById(int id);
    string GetPatientPNC(Patient patient);
    bool VerifyPassword(string password, string storedHash, string storedSalt);
}