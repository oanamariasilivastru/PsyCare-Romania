using backend.Dtos.Tests;

namespace backend.Service.Interfaces;

public interface ITestCompletionService
{
    Task CreateTestCompletion(TestCompletionDto dto);

    Task<List<TestCompletionResponseDto>> GetPatientTestCompletions(int patientId);

    Task<List<TestCompletionResponseDto>> GetPatientTestsByCode(int patientId, string code);

    Task<TestCompletionResponseDto?> GetTestCompletionById(int id);

    Task<List<TestHistoryDto>> GetTestHistory(int patientId);

    Task<bool> DeleteTestCompletion(int id);
}