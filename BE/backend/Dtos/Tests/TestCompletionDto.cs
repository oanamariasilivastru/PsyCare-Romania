namespace backend.Dtos.Tests;

public class TestCompletionDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string TestCode { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public string? ResultLabel { get; set; }
    public string? Severity { get; set; }
    public DateTime CompletedAt { get; set; }
    public List<TestAnswerDto>? Answers { get; set; }
}