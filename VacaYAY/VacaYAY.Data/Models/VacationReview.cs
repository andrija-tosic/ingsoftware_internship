using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class VacationReview
{
    [Key]
    public int Id { get; set; }
    public string? Comment { get; set; }
    [Required]
    public int VacationRequestRefId { get; set; }
    [Required]
    public required VacationRequest VacationRequest { get; set; }
    [Required]
    public required bool Approved { get; set; }
    [Required]
    public required Employee Reviewer { get; set; }

    public override string? ToString()
    {
        return 
$@"Review comment: {Comment}
Status: {(Approved ? "Approved" : "Rejected")}
Reviewed by: {Reviewer.FirstName} {Reviewer.LastName} ({Reviewer.Email})
";
    }
}
