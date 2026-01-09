using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Mood")]
public class Mood
{
    [Key, Column("Patient", Order = 0)]
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    [Key, Column("completion_date", Order = 1)]
    public DateTime Date { get; set; }

    [Required]
    [Column("score")]
    public byte Score { get; set; }

    public Mood() { }

    public Mood(Patient patient, DateTime date, byte score)
    {
        this.Patient = patient;
        this.PatientId = patient.Id;
        this.Date = date;
        this.Score = score;
    }

    public override string ToString()
    {
        return $"{Patient?.Name ?? "Unknown"} {Date:dd.MM.yyyy} {Score}/10";
    }
}