using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class SendMessageDto
{
    [Required]
    public int SenderId { get; set; }

    [Required]
    [StringLength(20)]
    public string SenderType { get; set; } = "patient";

    [Required]
    public int ReceiverId { get; set; }

    [Required]
    [StringLength(20)]
    public string ReceiverType { get; set; } = "psychologist";

    [Required]
    [StringLength(5000)]
    public string Content { get; set; } = string.Empty;
}