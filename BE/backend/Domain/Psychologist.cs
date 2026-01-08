namespace backend.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

[Table("Psychologist")]
public class Psychologist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    private int id;
    
    [Required, MaxLength(100)]
    [Column("name")]
    private string name;

    [Required, MaxLength(13)] 
    [Column("stamp_code")]
    private string doctor_stamp_code;

    [Required, MaxLength(128)]
    [Column("password")]
    private string password;

    [Required, MaxLength(64)]
    [Column("salt")]
    private string salt;

    public Psychologist(int id, string name, string doctorStampCode, string password, string salt)
    {
        this.id = id;
        this.name = name;
        this.doctor_stamp_code = doctorStampCode;
        this.password = password;
        this.salt = salt;
    }

    public int Id => id;

    public string Name => name;

    public string DoctorStampCode => doctor_stamp_code;

    public string Salt => salt;

    public string Password
    {
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"{name} {doctor_stamp_code}";
    }
}