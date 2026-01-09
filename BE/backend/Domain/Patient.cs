namespace backend.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

[Table("Patient")]
public class Patient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("IdentifierToken", TypeName = "uniqueidentifier")]
    public Guid? IdentifierToken { get; set; }

    [Required, MaxLength(128)]
    [Column("password")]
    public string Password { get; set; }

    [Required, MaxLength(64)]
    [Column("salt")]
    public string Salt { get; set; }
    
    public Patient() { }
    
    public Patient(string name, string password, string salt)
    {
        Name = name;
        Password = password;
        Salt = salt;
    }

    public Patient(string name, Guid? identifierToken, string password, string salt)
    {
        Name = name;
        IdentifierToken = identifierToken;
        Password = password;
        Salt = salt;
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}
