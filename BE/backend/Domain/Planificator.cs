using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Planificator")]
public class Planificator
{
    [Key, Column("Psychologist", Order = 0)]
    [ForeignKey(nameof(Psychologist))]
    public int PsychologistId { get; set; }

    public Psychologist Psychologist { get; set; } = null!;

    [Key, Column("Patient", Order = 1)]
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    [Key, Column("appointment_date", Order = 2)]
    public DateTime Date { get; set; }

    [Required]
    [Column("fee", TypeName = "decimal(6,2)")]
    public decimal Fee { get; set; }

    public Planificator() { }

    public Planificator(Psychologist psychologist, Patient patient, DateTime date, decimal fee)
    {
        this.Psychologist = psychologist;
        this.PsychologistId = psychologist.Id;
        this.Patient = patient;
        this.PatientId = patient.Id;
        this.Date = date;
        this.Fee = fee;
    }

    public override string ToString()
    {
        return $"{Psychologist?.Name ?? "Unknown"} {Patient?.Name ?? "Unknown"} {Date:dd.MM.yyyy HH:mm} €{Fee:0.00}";
    }
}