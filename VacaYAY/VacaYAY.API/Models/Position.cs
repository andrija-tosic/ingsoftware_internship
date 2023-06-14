namespace VacaYAY.API.Models;

public class Position
{
    public int Id { get; set; }
    public required string Caption { get; set; }
    public required string Description { get; set; }
    public required List<Employee> Employees { get; set; }
}
