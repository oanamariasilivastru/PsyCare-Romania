using backend.Dtos;
using backend.Service;
using backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/PSYCare/[controller]")]
    [Authorize]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService _moodService;

        public MoodController(IMoodService moodService)
        {
            _moodService = moodService ?? throw new ArgumentNullException(nameof(moodService));
        }
        
        [HttpPost]
        public IActionResult AddMood([FromBody] MoodDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Mood data is required");

                _moodService.AddMood(dto);
                return Ok(new { message = "Mood added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add mood", error = ex.Message });
            }
        }
        
        [HttpGet("{patientId}")]
        public ActionResult<List<MoodResponseDto>> GetMoods(int patientId)
        {
            try
            {
                var moods = _moodService.GetMoods(patientId);
                return Ok(moods);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get moods", error = ex.Message });
            }
        }
    }
}