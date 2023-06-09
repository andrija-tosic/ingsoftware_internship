namespace VacaYAY.API.Models;

public class LeaveType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
}
