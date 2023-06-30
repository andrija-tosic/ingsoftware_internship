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
    [Required, Range(0, int.MaxValue)]
    [DisplayName("Number of last year's days off")]
    public required int LastYearsDaysOffNumber { get; set; }
    [Required]
    [DisplayName("Position")]
    public required Position Position { get; set; }
    [Required]
    [DisplayName("Employment start date")]
    [DataType(DataType.Date)]
    public required DateTime EmploymentStartDate { get; set; }
    [DisplayName("Employment end date")]
    [DataType(DataType.Date)]
    public DateTime? EmploymentEndDate { get; set; }
    [Required]
    [DisplayName("Date of insertion")]
    [DataType(DataType.Date)]
    public required DateTime InsertDate { get; set; }
    [DisplayName("Date of deletion")]
    [DataType(DataType.Date)]
    public DateTime? DeleteDate { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
    public required List<VacationReview> VacationReviews { get; set; }
    public required List<Contract> Contracts { get; set; }
}
