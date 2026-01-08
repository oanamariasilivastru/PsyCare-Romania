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
    public int id;
    
    [Required, MaxLength(100)]
    [Column("name")]
    public string name;

    [Required]
    [Column("identifier_token")]
    public Guid? identifierToken;

    [Required, MaxLength(128)]
    [Column("password")]
    public string password;

    [Required, MaxLength(64)]
    [Column("salt")]
    public string salt;

    public Patient() { }
    public Patient(string name, string password, string salt)
    {
        this.name = name;
        this.password = password;
        this.salt = salt;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Guid? IdentifierToken
    {
        get => identifierToken;
        set => identifierToken = value;
    }

    public string Password
    {
        get => password;
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Salt
    {
        get => salt;
        set => salt = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"{name} [token: {identifierToken}]";
    }
}
