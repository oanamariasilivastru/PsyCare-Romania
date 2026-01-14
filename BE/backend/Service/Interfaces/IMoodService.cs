using backend.Dtos;

namespace backend.Service.Interfaces;

public interface IMoodService
{
    void AddMood(MoodDto moodDto);
    List<MoodResponseDto> GetMoods(int patientId);
}