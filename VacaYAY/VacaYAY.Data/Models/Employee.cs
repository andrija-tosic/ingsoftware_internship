using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class Employee : IdentityUser
{
    [Required, MaxLength(50)]
    public required string FirstName { get; set; }
    [Required, MaxLength(50)]
    public required string LastName { get; set; }
    [Required, MaxLength(512)]
    public required string Address { get; set; }
    [Required, MaxLength(50)]
    public required string IdNumber { get; set; }
    [Required, Range(0, int.MaxValue)]
    public required int DaysOffNumber { get; set; }
    [Required]
    public required Position Position { get; set; }
    [Required]
    public required DateTime EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    [Required]
    public required DateTime InsertDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
}

