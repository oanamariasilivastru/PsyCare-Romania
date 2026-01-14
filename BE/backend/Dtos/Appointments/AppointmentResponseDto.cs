namespace backend.Dtos;

public class AppointmentResponseDto
{
    public int PsychologistId { get; set; }
    public string PsychologistName { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Fee { get; set; }
}