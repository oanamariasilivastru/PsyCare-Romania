using backend.Dtos;
using backend.Dtos.Tests;
using backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestCompletionController : ControllerBase
    {
        private readonly ITestCompletionService _testCompletionService;

        public TestCompletionController(ITestCompletionService testCompletionService)
        {
            _testCompletionService = testCompletionService ?? throw new ArgumentNullException(nameof(testCompletionService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestCompletion([FromBody] TestCompletionDto testCompletionDto)
        {
            if (testCompletionDto == null)
                return BadRequest("Test completion data is required");

            await _testCompletionService.CreateTestCompletion(testCompletionDto);
            return Ok(new { message = "Test completion saved successfully" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestCompletionResponseDto>> GetTestCompletionById(int id)
        {
            var testCompletion = await _testCompletionService.GetTestCompletionById(id);

            if (testCompletion == null)
                return NotFound(new { message = "Test completion not found" });

            return Ok(testCompletion);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<TestCompletionResponseDto>>> GetPatientTestCompletions(int patientId)
        {
            var completions = await _testCompletionService.GetPatientTestCompletions(patientId);
            return Ok(completions);
        }

        [HttpGet("patient/{patientId}/test/{testCode}")]
        public async Task<ActionResult<List<TestCompletionResponseDto>>> GetPatientTestsByCode(
            int patientId,
            string testCode)
        {
            var completions = await _testCompletionService.GetPatientTestsByCode(patientId, testCode);
            return Ok(completions);
        }

        [HttpGet("patient/{patientId}/history")]
        public async Task<ActionResult<List<TestHistoryDto>>> GetTestHistory(int patientId)
        {
            var history = await _testCompletionService.GetTestHistory(patientId);
            return Ok(history);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestCompletion(int id)
        {
            var success = await _testCompletionService.DeleteTestCompletion(id);

            if (!success)
                return NotFound(new { message = "Test completion not found" });

            return Ok(new { message = "Test completion deleted successfully" });
        }
    }
}
