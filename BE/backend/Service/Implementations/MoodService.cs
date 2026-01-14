using backend.Domain;
using backend.Dtos;
using backend.Repo;
using backend.Repo.Interfaces;
using backend.Service.Interfaces;

namespace backend.Service
{
    public class MoodService : IMoodService
    {
        private readonly IMoodRepository moodRepo;
        private readonly IPatientRepository patientRepo;

        public MoodService(IMoodRepository moodRepo, IPatientRepository patientRepo)
        {
            this.moodRepo = moodRepo ?? throw new ArgumentNullException(nameof(moodRepo));
            this.patientRepo = patientRepo ?? throw new ArgumentNullException(nameof(patientRepo));
        }

        public void AddMood(MoodDto moodDto)
        {
            if (moodDto == null) throw new ArgumentNullException(nameof(moodDto));

            var patient = patientRepo.GetPatientById(moodDto.PatientId);
            if (patient == null) throw new Exception("Patient not found");

            var mood = new Mood(patient, moodDto.Date, moodDto.Score);
            moodRepo.AddMood(mood);
        }

        public List<MoodResponseDto> GetMoods(int patientId)
        {
            var moods = moodRepo.GetMoods(patientId);

            return moods.Select(m => new MoodResponseDto
            {
                Score = m.Score,
                CompletionDate = m.Date,
                PatientId = m.PatientId
            }).ToList();
        }
    }
}