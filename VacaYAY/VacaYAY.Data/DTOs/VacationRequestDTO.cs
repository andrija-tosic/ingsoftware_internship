using VacaYAY.Data.Models;

namespace VacaYAY.Data.DTOs;

public class VacationRequestDTO
{
    public int Id { get; set; }
    public required Employee Employee { get; set; }
    public required LeaveType LeaveType { get; set; }
    public string? Comment { get; set; }
    public required VacationReview VacationReview { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public override string ToString()
    {
        return
$@"Leave type: {LeaveType.Name}
Request comment: {Comment}
Start date: {StartDate.Date.ToShortDateString()}
End date: {EndDate.Date.ToShortDateString()}
Requested by: {Employee.FirstName} {Employee.LastName} ({Employee.Email})
";
    }
}
