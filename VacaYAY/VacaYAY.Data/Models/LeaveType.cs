using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class LeaveType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
}