using backend.Domain;

namespace backend.Service.Interfaces;

public interface IPatientService
{
    void AddPatient(Patient patient, string pnc);
    Patient? GetPatient(string name);
    Patient? GetPatientById(int id);
    string GetPatientPNC(Patient patient);
    string? LoginPatient(string name, string password);
}