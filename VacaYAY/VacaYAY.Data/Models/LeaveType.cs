using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class LeaveType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
}