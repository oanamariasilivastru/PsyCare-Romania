using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Planificator")]
public class Planificator

{   
    public Planificator()
    {
    }

    public Planificator(Psychologist psychologist, Patient patient, DateTime date, decimal fee)
    {
        this.Psychologist = psychologist;
        this.Patient = patient;
        this.Date = date;
        this.Fee = fee;
    }
    [ForeignKey(nameof(Psychologist))]
    [Column("Psychologist")]
    public Psychologist Psychologist { get; set; }

    [ForeignKey(nameof(Patient))]
    [Column("Patient")]
    public Patient Patient { get; set; }

    [Column("appointment_date")]
    public DateTime Date { get; set; }
    
    [Column("fee", TypeName = "decimal(6,2)")]
    public decimal Fee {get; set;}

    public override string ToString()
    {
        return $"{Psychologist.Name} {Patient.Name} {Date:dd.MM.yyyy HH:mm} €{Fee:0.00}";
    }
}