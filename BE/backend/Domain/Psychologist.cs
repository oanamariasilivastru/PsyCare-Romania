namespace backend.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

[Table("Psychologist")]
public class Psychologist
{
    public Psychologist()
    {
    }

    public Psychologist(string name, string password, string salt)
    {
        this.Name = name;
        this.Password = password;
        this.Salt = salt;
    }

    public Psychologist(string name, Guid? identifierToken, string password, string salt)
    {
        this.Name = name;
        this.IdentifierToken = identifierToken;
        this.Password = password;
        this.Salt = salt;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column("name")]
    public string Name { get ; set ; }

    [Required]
    [Column("IdentifierToken")]
    public Guid? IdentifierToken { get; set; }

    [Required, MaxLength(128)]
    [Column("password")]
    public string Password { get; set; }

    [Required, MaxLength(64)]
    [Column("salt")]
    public string Salt { get ; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}
