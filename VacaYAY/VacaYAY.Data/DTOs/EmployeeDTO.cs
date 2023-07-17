using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using VacaYAY.Data.Models;

namespace VacaYAY.Data.DTOs;

public class EmployeeDTO
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
    [Required]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "ID number must contain numbers only.")]
    public required string IdNumber { get; set; }
    [Required]
    public required int DaysOffNumber { get; set; }
    public required int LastYearsDaysOffNumber { get; set; }
    public required int PositionId { get; set; }
    public required DateTime EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public required DateTime InsertDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    public Contract Contract { get; set; } = default!;

    public static implicit operator EmployeeDTO(Employee v)
    {
        return new EmployeeDTO
        {
            Id = v.Id,
            FirstName = v.FirstName,
            LastName = v.LastName,
            Address = v.Address,
            DaysOffNumber = v.DaysOffNumber,
            LastYearsDaysOffNumber = v.LastYearsDaysOffNumber,
            Email = v.Email,
            EmploymentStartDate = v.EmploymentStartDate,
            EmploymentEndDate = v.EmploymentEndDate,
            DeleteDate = v.DeleteDate,
            IdNumber = v.IdNumber,
            InsertDate = v.InsertDate,
            PositionId = v.Position.Id
        };
    }
}

