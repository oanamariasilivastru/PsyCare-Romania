using backend.Domain;
using backend.Dtos.Tests;
using backend.Repo;
using backend.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PSYCare.Repositories
{
    public class TestCompletionRepository : ITestCompletionRepository
    {
        private readonly PSYCareDbContext _context;

        public TestCompletionRepository(PSYCareDbContext context)
        {
            _context = context;
        }

        public async Task<TestCompletion> CreateTestCompletionAsync(TestCompletion testCompletion)
        {
            _context.TestCompletions.Add(testCompletion);
            await _context.SaveChangesAsync();
            return testCompletion;
        }

        public async Task<List<TestCompletion>> GetPatientTestCompletionsAsync(int patientId)
        {
            return await _context.TestCompletions
                .Where(tc => tc.PatientId == patientId)
                .OrderByDescending(tc => tc.CompletedAt)
                .ToListAsync();
        }

        public async Task<List<TestCompletion>> GetPatientTestsByCodeAsync(int patientId, string testCode)
        {
            return await _context.TestCompletions
                .Where(tc => tc.PatientId == patientId && tc.TestCode == testCode)
                .OrderByDescending(tc => tc.CompletedAt)
                .ToListAsync();
        }

        public async Task<TestCompletion?> GetTestCompletionByIdAsync(int id)
        {
            return await _context.TestCompletions
                .Include(tc => tc.Patient)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<List<TestHistoryDto>> GetTestHistoryAsync(int patientId)
        {
            var completions = await _context.TestCompletions
                .Where(tc => tc.PatientId == patientId)
                .GroupBy(tc => tc.TestCode)
                .Select(g => new TestHistoryDto
                {
                    TestCode = g.Key,
                    TestName = GetTestName(g.Key),
                    CompletionCount = g.Count(),
                    LastCompletedAt = g.Max(tc => tc.CompletedAt),
                    LastScore = g.OrderByDescending(tc => tc.CompletedAt).First().TotalScore,
                    LastSeverity = g.OrderByDescending(tc => tc.CompletedAt).First().Severity
                })
                .ToListAsync();

            return completions;
        }

        public async Task<bool> DeleteTestCompletionAsync(int id)
        {
            var testCompletion = await _context.TestCompletions.FindAsync(id);
            if (testCompletion == null)
                return false;

            _context.TestCompletions.Remove(testCompletion);
            await _context.SaveChangesAsync();
            return true;
        }

        private static string GetTestName(string testCode)
        {
            return testCode.ToLower() switch
            {
                "phq9" => "Patient Health Questionnaire-9",
                "gad7" => "Generalized Anxiety Disorder-7",
                "pcl5" => "PTSD Checklist for DSM-5",
                "rosenberg" => "Rosenberg Self-Esteem Scale",
                "pss" => "Perceived Stress Scale",
                _ => testCode
            };
        }
    }
}
