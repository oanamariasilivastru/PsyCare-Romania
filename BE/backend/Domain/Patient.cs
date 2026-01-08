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
    private int id;
    
    [Required, MaxLength(100)]
    [Column("name")]
    private string name;

    [Required, MaxLength(13)] 
    [Column("pnc")]
    private string pnc;

    [Required, MaxLength(128)]
    [Column("password")]
    private string password;

    [Required, MaxLength(64)]
    [Column("salt")]
    private string salt;

    public Patient(int id, string name, string pnc, string password, string salt)
    {
        this.id = id;
        this.name = name;
        this.pnc = pnc;
        this.password = password;
        this.salt = salt;
    }

    public int Id => id;

    public string Name => name;

    public string Pnc => pnc;

    public string Salt => salt;

    public string Password
    {
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"{name} {pnc}";
    }
}