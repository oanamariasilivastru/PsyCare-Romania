using backend.Domain;

namespace backend.Repo;

public interface IRepo
{
    public void addPatient(Patient p, string pnc);
    public void addPsychologist(Psychologist p, string code);
}