using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.DTOs;

public class ContractDTO
{
    public int Id { get; set; }
    [Required, MinLength(6), MaxLength(6)]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Contract number must contain numbers only.")]
    [Display(Name = "Contract number")]
    public required string Number { get; set; }
    [Required]
    public required string EmployeeId { get; set; }
    [Required]
    public required int ContractTypeId { get; set; }
    [Required]
    public required IFormFile ContractFile { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Contract start date")]
    public required DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Contract end date")]
    public DateTime? EndDate { get; set; }
}
