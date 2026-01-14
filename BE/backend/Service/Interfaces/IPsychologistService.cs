using backend.Domain;

namespace backend.Service.Interfaces;


public interface IPsychologistService
{
    void AddPsychologist(Psychologist psychologist, string code);
    Psychologist? GetPsychologist(string name);
    Psychologist? GetPsychologistById(int id);
    string GetPsychologistStamp(Psychologist psychologist);
    string? LoginPsychologist(string name, string password);
}