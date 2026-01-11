using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain
{
    [Table("Mood")]
    public class Mood
    {
        public Mood() { }

        public Mood(Patient patient, DateTime date, byte score)
        {
            Patient = patient;
            PatientId = patient.Id;
            Date = date;
            Score = score;
        }

        [Column("Patient")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [Column("completion_date")]
        public DateTime Date { get; set; }

        [Column("score")]
        public byte Score { get; set; }

        public override string ToString()
        {
            return $"{Patient?.Name} {Date:dd.MM.yyyy} {Score}/10";
        }
    }
}