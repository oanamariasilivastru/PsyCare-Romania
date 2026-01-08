using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Planificator")]
public class Planificator

{   [ForeignKey(nameof(Psychologist))]
    [Column("Psychologist")]
    private Psychologist psychologist;
    
    [ForeignKey(nameof(Patient))]
    [Column("Patient")]
    private Patient patient;
    
    [Column("appointment_date")]
    private DateTime date;
    
    [Column("fee", TypeName = "decimal(6,2)")]
    private decimal fee;

    public Planificator(Psychologist psychologist, Patient patient, DateTime date, decimal fee)
    {
        this.psychologist = psychologist;
        this.patient = patient;
        this.date = date;
        this.fee = fee;
    }

    public Psychologist Psychologist => psychologist;

    public Patient Patient => patient;

    public DateTime Date => date;

    public decimal Fee => fee;

    public override string ToString()
    {
        return $"{psychologist.Name} {patient.Name} {date:dd.MM.yyyy HH:mm} €{fee:0.00}";
    }
}