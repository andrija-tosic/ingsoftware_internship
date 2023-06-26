using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class ContractType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
}
