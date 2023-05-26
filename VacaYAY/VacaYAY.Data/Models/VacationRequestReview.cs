using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacaYAY.Data.Models;

public class VacationRequestReview
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(512)]
    public required string Comment { get; set; }
    [Required]
    [ForeignKey(nameof(VacationRequest))]
    public int VacationRequestRefId { get; set; }
    [Required]
    public required VacationRequest VacationRequest { get; set; }
    [Required]
    public required LeaveType LeaveType { get; set; }
    [Required]
    public required bool Approved { get; set; }

}