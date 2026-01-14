using backend.Domain;
using backend.Dtos.Tests;

namespace backend.Repo.Interfaces;

public interface ITestCompletionRepository
{
    Task<TestCompletion> CreateTestCompletionAsync(TestCompletion testCompletion);
    Task<List<TestCompletion>> GetPatientTestCompletionsAsync(int patientId);
    Task<List<TestCompletion>> GetPatientTestsByCodeAsync(int patientId, string testCode);
    Task<TestCompletion?> GetTestCompletionByIdAsync(int id);
    Task<List<TestHistoryDto>> GetTestHistoryAsync(int patientId);
    Task<bool> DeleteTestCompletionAsync(int id);
}