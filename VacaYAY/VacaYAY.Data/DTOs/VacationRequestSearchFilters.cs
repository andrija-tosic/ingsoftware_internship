namespace VacaYAY.Data.DTOs;

public class VacationRequestSearchFilters
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public int? LeaveTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
