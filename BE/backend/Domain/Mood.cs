using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;
[Table("Mood")]
public class Mood
{
    [ForeignKey(nameof(Patient))]
    [Column("Patient")]
    private Patient patient;
    
    [Column("completion_date")]
    private DateTime date;
    
    [Column("score")]
    private byte score;

    public Mood(Patient patient, DateTime date, byte score)
    {
        this.patient = patient;
        this.date = date;
        this.score = score;
    }

    public Patient Patient => patient;

    public DateTime Date => date;

    public byte Score => score;

    public override string ToString()
    {
        return $"{patient.Name} {date} {score}/10";
    }
}