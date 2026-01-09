using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;
[Table("Mood")]
public class Mood
{
    public Mood()
    {
    }

    public Mood(Patient patient, DateTime date, byte score)
    {
        this.Patient = patient;
        this.Date = date;
        this.Score = score;
    }

    [ForeignKey(nameof(Patient))]
    [Column("Patient")]
    public Patient Patient {get; set; }

    [Column("completion_date")]
    public DateTime Date { get; set; }
    
    [Column("score")]
    public byte Score { get; set; }

    public override string ToString()
    {
        return $"{Patient.Name} {Date} {Score}/10";
    }
}