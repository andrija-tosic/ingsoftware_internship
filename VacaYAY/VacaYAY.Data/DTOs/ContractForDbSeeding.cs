namespace VacaYAY.Data.DTOs;

public class ContractForDbSeeding
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string EmployeeId { get; set; }
    public required int TypeId { get; set; }
    public required string DocumentUrl { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}
