using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("TestCompletion")]
public class TestCompletion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [Column("patient_id")] public int PatientId { get; set; }

    [ForeignKey("PatientId")] public Patient? Patient { get; set; }

    [Required]
    [StringLength(20)]
    [Column("test_code")]
    public string TestCode { get; set; } = string.Empty; 

    [Required] [Column("total_score")] public int TotalScore { get; set; }

    [StringLength(100)]
    [Column("result_label")]
    public string? ResultLabel { get; set; }

    [StringLength(20)]
    [Column("severity")]
    public string? Severity { get; set; }

    [Column("completed_at")] public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    [Column("answers")] public string? Answers { get; set; }
}