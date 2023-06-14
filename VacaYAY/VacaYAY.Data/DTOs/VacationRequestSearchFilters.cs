using System.ComponentModel;

namespace VacaYAY.Data.DTOs;

public class VacationRequestSearchFilters
{
    [DisplayName("Employee full name")]
    public string? EmployeeFullName { get; set; }
    public int? LeaveTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
