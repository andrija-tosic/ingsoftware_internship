using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
    [DisplayName("Contract document")]
    public required string DocumentUrl { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Contract start date")]
    public required DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Contract end date")]
    public DateTime? EndDate { get; set; }
}
