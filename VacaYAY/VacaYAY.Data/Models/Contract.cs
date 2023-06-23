using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

[Index(nameof(Number), IsUnique = true)]
public class Contract
{
    [Key]
    public int Id { get; set; }
    [Required, MinLength(6), MaxLength(6)]
    public required string Number { get; set; }
    [Required]
    public required Employee Employee { get; set; }
    [Required]
    public required ContractType Type { get; set; }
    [Required]
    [Url]
    public required string DocumentUrl { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
}
