using backend.Domain;
using backend.Dtos.Tests;
using backend.Repo.Interfaces;
using backend.Service.Interfaces;
using System.Text.Json;

namespace backend.Service
{
    public class TestCompletionService : ITestCompletionService
    {
        private readonly ITestCompletionRepository repo;
        private readonly IPatientRepository patientRepo;

        public TestCompletionService(
            ITestCompletionRepository repo,
            IPatientRepository patientRepo)
        {
            this.repo = repo;
            this.patientRepo = patientRepo;
        }

        public async Task CreateTestCompletion(TestCompletionDto dto)
        {
            var tc = new TestCompletion
            {
                PatientId = dto.PatientId,
                TestCode = dto.TestCode.ToLower(),
                TotalScore = dto.TotalScore,
                ResultLabel = dto.ResultLabel,
                Severity = dto.Severity?.ToLower(),
                CompletedAt = DateTime.UtcNow,
                Answers = JsonSerializer.Serialize(dto.Answers)
            };

            await repo.CreateTestCompletionAsync(tc);
        }

        public async Task<List<TestCompletionResponseDto>> GetPatientTestCompletions(int patientId)
        {
            var list = await repo.GetPatientTestCompletionsAsync(patientId);
            return await MapList(list);
        }

        public async Task<List<TestCompletionResponseDto>> GetPatientTestsByCode(int patientId, string code)
        {
            var list = await repo.GetPatientTestsByCodeAsync(patientId, code.ToLower());
            return await MapList(list);
        }

        public async Task<TestCompletionResponseDto?> GetTestCompletionById(int id)
        {
            var tc = await repo.GetTestCompletionByIdAsync(id);
            if (tc == null)
                return null;

            return await Map(tc);
        }

        public async Task<List<TestHistoryDto>> GetTestHistory(int patientId)
        {
            return await repo.GetTestHistoryAsync(patientId);
        }

        public async Task<bool> DeleteTestCompletion(int id)
        {
            return await repo.DeleteTestCompletionAsync(id);
        }

        private async Task<List<TestCompletionResponseDto>> MapList(List<TestCompletion> list)
        {
            var result = new List<TestCompletionResponseDto>();
            foreach (var tc in list)
                result.Add(await Map(tc));

            return result;
        }

        private async Task<TestCompletionResponseDto> Map(TestCompletion tc)
        {
            var patient = patientRepo.GetPatientById(tc.PatientId);

            return new TestCompletionResponseDto
            {
                Id = tc.Id,
                PatientId = tc.PatientId,
                PatientName = patient?.Name ?? "Unknown Patient",
                TestCode = tc.TestCode,
                TestName = tc.TestCode.ToUpper(),
                TotalScore = tc.TotalScore,
                ResultLabel = tc.ResultLabel,
                Severity = tc.Severity,
                CompletedAt = tc.CompletedAt
            };
        }
    }
}
