using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
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
    [Required, MaxLength(320)]
    public required string Email { get; set; }
    [Required, MaxLength(72)]
    public required string PasswordHash { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
}

