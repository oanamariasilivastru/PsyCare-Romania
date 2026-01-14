using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Tests;

public class CreateTestCompletionDto
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    [StringLength(20)]
    public string TestCode { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000)]
    public int TotalScore { get; set; }

    [StringLength(100)]
    public string? ResultLabel { get; set; }

    [StringLength(20)]
    public string? Severity { get; set; }

    [Required]
    public List<TestAnswerDto> Answers { get; set; } = new List<TestAnswerDto>();
}
