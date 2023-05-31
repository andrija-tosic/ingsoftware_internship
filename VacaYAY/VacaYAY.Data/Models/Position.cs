using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class Position // : IdentityRole<int>
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public required string Caption { get; set; }
    [Required, MaxLength(512)]
    public required string Description { get; set; }
    public required List<Employee> Employees { get; set; }
}
