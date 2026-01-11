using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace backend.Domain;

[Table("Patient")]
public class Patient
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column("name")]
    public string Name { get; set; }

    [Column("IdentifierToken", TypeName = "uniqueidentifier")]
    public Guid? IdentifierToken { get; set; }

    [Required, MaxLength(128)]
    [Column("password")]
    public string Password { get; set; }

    [Required, MaxLength(64)]
    [Column("salt")]
    public string Salt { get; set; }
    
    public ICollection<Mood> Moods { get; set; } = new List<Mood>();
    public ICollection<Planificator> Planificari { get; set; } = new List<Planificator>();

    public override string ToString() => Name;
}