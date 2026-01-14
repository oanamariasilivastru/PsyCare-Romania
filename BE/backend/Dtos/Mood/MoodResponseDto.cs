namespace backend.Dtos
{
    public class MoodResponseDto
    {
        public int Score { get; set; }
        public DateTime CompletionDate { get; set; }
        public int PatientId { get; set; }
    }
}