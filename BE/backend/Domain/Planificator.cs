using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Planificator")]
public class Planificator
{
    [Column("Psychologist")]
    public int PsychologistId { get; set; }
    
    [ForeignKey(nameof(PsychologistId))]
    public Psychologist Psychologist { get; set; }

    [Column("Patient")]
    public int PatientId { get; set; }
    
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; }

    [Column("appointment_date")]
    public DateTime Date { get; set; }

    [Column("fee", TypeName = "decimal(6,2)")]
    public decimal Fee { get; set; }

    public Planificator() { }

    public Planificator(Psychologist psychologist, Patient patient, DateTime date, decimal fee)
    {
        Psychologist = psychologist;
        PsychologistId = psychologist.Id;
        Patient = patient;
        PatientId = patient.Id;
        Date = date;
        Fee = fee;
    }

    public override string ToString()
    {
        return $"{Psychologist?.Name} {Patient?.Name} {Date:dd.MM.yyyy HH:mm} €{Fee:0.00}";
    }
}