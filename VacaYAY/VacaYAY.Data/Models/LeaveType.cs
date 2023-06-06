using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

[Index(nameof(Name), IsUnique = true)]
[DisplayName("Leave type")]
public class LeaveType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
}