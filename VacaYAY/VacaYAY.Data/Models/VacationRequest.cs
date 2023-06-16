using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Models;

public class VacationRequest
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required Employee Employee { get; set; }
    [Required]
    public required LeaveType LeaveType { get; set; }
    [Required]
    public required string Comment { get; set; }
    public VacationReview? VacationReview { get; set; }
    [Required]
    [DisplayName("Start date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }
    [Required]
    [DisplayName("End date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    public override string ToString()
    {
        return
$@"Leave type: {LeaveType.Name}
Request comment: {Comment}
Start date: {StartDate.Date.ToShortDateString()}
End date: {EndDate.Date.ToShortDateString()}
Requested by: {Employee.FirstName} {Employee.LastName} ({Employee.Email})
";
    }
}
