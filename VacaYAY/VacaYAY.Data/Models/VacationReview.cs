using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class VacationReview
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(512)]
    public required string Comment { get; set; }
    [Required]
    public int VacationRequestRefId { get; set; }
    [Required]
    public required VacationRequest VacationRequest { get; set; }
    [Required]
    public required bool Approved { get; set; }
    [Required]
    public required Employee Reviewer { get; set; }
}
