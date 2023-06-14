namespace VacaYAY.API.Models;

public class VacationRequest
{
    public int Id { get; set; }
    public required Employee Employee { get; set; }
    public required LeaveType LeaveType { get; set; }
    public required string Comment { get; set; }
    public VacationReview? VacationReview { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}
