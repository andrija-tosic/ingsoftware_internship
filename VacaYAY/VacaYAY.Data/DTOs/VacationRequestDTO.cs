using VacaYAY.Data.Models;

namespace VacaYAY.Data.DTOs;

public class VacationRequestDTO
{
    public int Id { get; set; }
    public required Employee Employee { get; set; }
    public required LeaveType LeaveType { get; set; }
    public required string Comment { get; set; }
    public required VacationReview VacationReview { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}
