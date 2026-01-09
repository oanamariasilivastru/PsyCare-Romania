using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Psychologist")]
public class Psychologist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("salt")]
    [StringLength(64)]
    public string Salt { get; set; } = string.Empty;

    [Column("IdentifierToken", TypeName = "uniqueidentifier")]
    public Guid? IdentifierToken { get; set; }  // Nullable pentru că în SQL e NULL

    public Psychologist() { }

    public Psychologist(string name, string password, string salt)
    {
        Name = name;
        Password = password;
        Salt = salt;
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}