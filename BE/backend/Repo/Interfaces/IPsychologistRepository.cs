using backend.Domain;

namespace backend.Repo.Interfaces;

public interface IPsychologistRepository
{
    void AddPsychologist(Psychologist psychologist, string code);
    Psychologist? GetPsychologist(string name);
    Psychologist? GetPsychologistById(int id);
    string GetPsychologistStamp(Psychologist psychologist);
    bool VerifyPassword(string password, string storedHash, string storedSalt);
}