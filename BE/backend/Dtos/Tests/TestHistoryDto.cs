namespace backend.Dtos.Tests;


public class TestHistoryDto
{
    public string TestCode { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public int CompletionCount { get; set; }
    public DateTime? LastCompletedAt { get; set; }
    public int? LastScore { get; set; }
    public string? LastSeverity { get; set; }
}