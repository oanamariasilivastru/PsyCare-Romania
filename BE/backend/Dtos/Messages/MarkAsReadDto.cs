using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class MarkAsReadDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string UserType { get; set; } = string.Empty;

    [Required]
    public int OtherUserId { get; set; }

    [Required]
    public string OtherUserType { get; set; } = string.Empty;
}