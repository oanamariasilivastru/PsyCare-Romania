using backend.Domain;
using backend.Repo.Interfaces;

namespace backend.Repo
{
    public class MoodRepository : IMoodRepository
    {
        private readonly PSYCareDbContext dbContext;

        public MoodRepository(PSYCareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void AddMood(Mood mood)
        {
            if (mood == null) throw new ArgumentNullException(nameof(mood));
            dbContext.Moods.Add(mood);
            dbContext.SaveChanges();
        }

        public List<Mood> GetMoods(int patientId)
        {
            return dbContext.Moods
                .Where(m => m.PatientId == patientId)
                .OrderBy(m => m.Date)
                .ToList();
        }

        public Mood? GetMoodById(int patientId, DateTime date)
        {
            return dbContext.Moods
                .FirstOrDefault(m => m.PatientId == patientId && m.Date == date);
        }
    }
}