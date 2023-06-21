namespace VacaYAY.API.Models;

public class VacationReview
{
    public int Id { get; set; }
    public string? Comment { get; set; }
    public int VacationRequestRefId { get; set; }
    public required VacationRequest VacationRequest { get; set; }
    public required bool Approved { get; set; }
    public required Employee Reviewer { get; set; }
}
