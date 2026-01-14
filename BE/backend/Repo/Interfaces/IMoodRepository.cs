using backend.Domain;

namespace backend.Repo.Interfaces;

public interface IMoodRepository
{
    void AddMood(Mood mood);
    List<Mood> GetMoods(int patientId);
    Mood? GetMoodById(int patientId, DateTime date);
}