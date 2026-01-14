using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain;

[Table("Message")]
public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [Required]
    [StringLength(20)]
    [Column("sender_type")]
    public string SenderType { get; set; } = "patient"; // 'patient' or 'psychologist'

    [Required]
    [Column("receiver_id")]
    public int ReceiverId { get; set; }

    [Required]
    [StringLength(20)]
    [Column("receiver_type")]
    public string ReceiverType { get; set; } = "psychologist";

    [Required]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("sent_at")]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [Column("read_at")]
    public DateTime? ReadAt { get; set; }

    [Column("is_read")]
    public bool IsRead { get; set; } = false;
}