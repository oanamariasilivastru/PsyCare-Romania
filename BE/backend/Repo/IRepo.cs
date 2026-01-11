using backend.Domain;

namespace backend.Repo;

public interface IRepo
{
    public void addPatient(Patient p, string pnc);
    public void addPsychologist(Psychologist p, string code);
    
    public Patient? getPatient(string name);
    public Psychologist? getPsychologist(string name);

    public string getPatientPNC(Patient p);

    public string getPsychologistStamp(Psychologist p);
    public bool verifyPassword(string password, string storedHash, string storedSalt);
    public void addMood(Mood mood);
    public List<Mood> getMoods(Patient p);

    public void addAppointment(Planificator p);
    public List<Planificator> getPlanificatorsPatient(Patient p);
    public List<Planificator> getPlanificatorsPsychologist(Psychologist p);

}