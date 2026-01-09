using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain
{
    [Table("VaultIdentifier")]
    public class VaultIdentifier
    {
        [Key]
        public Guid Token { get; set; }

        [Required]
        [Column("encrypted_value")]
        public byte[] EncryptedValue { get; set; } = null!;

        [Required]
        [Column("data_type")]
        [MaxLength(30)]
        public string DataType { get; set; } = null!;

        [Required]
        [Column("retention_until")]
        public DateTime RetentionUntil { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}