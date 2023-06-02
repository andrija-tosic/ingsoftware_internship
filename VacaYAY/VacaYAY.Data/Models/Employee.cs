using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

[Index(nameof(IdNumber), IsUnique = true)]
[Index(nameof(FirstName), nameof(LastName), nameof(EmploymentStartDate), nameof(EmploymentEndDate))]
public class Employee : IdentityUser
{
    [Required, MaxLength(50)]
    [DisplayName("First name")]
    public required string FirstName { get; set; }
    [Required, MaxLength(50)]
    [DisplayName("Last name")]
    public required string LastName { get; set; }

    [Required, MaxLength(512)]
    [DisplayName("Address")]
    public required string Address { get; set; }
    [Required, MaxLength(50)]
    [DisplayName("ID Number")]
    public required string IdNumber { get; set; }
    [Required, Range(0, int.MaxValue)]
    [DisplayName("Number of days off")]
    public required int DaysOffNumber { get; set; }
    [Required]
    [DisplayName("Position")]
    public required Position Position { get; set; }
    [Required]
    [DisplayName("Employment start date")]
    public required DateTime EmploymentStartDate { get; set; }
    [DisplayName("Employment end date")]
    public DateTime? EmploymentEndDate { get; set; }
    [Required]
    [DisplayName("Date of insertion")]
    public required DateTime InsertDate { get; set; }
    [DisplayName("Date of deletion")]
    public DateTime? DeleteDate { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
}
