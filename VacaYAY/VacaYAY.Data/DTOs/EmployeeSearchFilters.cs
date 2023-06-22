namespace VacaYAY.Data.DTOs;

public class EmployeeSearchFilters
{
    public required string EmployeeFullName { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int[]? Positions { get; set; }
}
