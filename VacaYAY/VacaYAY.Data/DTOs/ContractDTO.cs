using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.DTOs;

public class ContractDTO
{
    public int Id { get; set; }
    [Required, MinLength(6), MaxLength(6)]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Contract number must contain numbers only.")]
    [DisplayName("Contract number")]
    public required string Number { get; set; }
    [Required]
    public required string EmployeeId { get; set; }
    [Required]
    public required int ContractTypeId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Contract start date")]
    public required DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Contract end date")]
    public DateTime? EndDate { get; set; }
    public IFormFile? ContractFile { get; set; } = default!;
}
