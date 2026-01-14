namespace backend.Dtos;

public class AppointmentDto
{
    public int PatientId { get; set; }
    public int PsychologistId { get; set; }
    public DateTime Date { get; set; }
    public decimal Fee { get; set; }
}