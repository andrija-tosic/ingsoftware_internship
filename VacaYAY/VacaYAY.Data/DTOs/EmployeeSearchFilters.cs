namespace VacaYAY.Data.DTOs;

public class EmployeeSearchFilters
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int NumberOfFakeEmployeesToGenerate { get; set; }
}
