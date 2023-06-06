using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacaYAY.Data.Models;

public class VacationRequest
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required Employee Employee { get; set; }
    [Required]
    public required string Comment { get; set; }
    [ForeignKey(nameof(VacationRequestReview))]
    public int VacationReviewRefId { get; set; }
    public VacationRequestReview? VacationReview { get; set; }
    [Required]
    public required DateTime StartDate { get; set; }
    [Required]
    public required DateTime EndDate { get; set; }
}
