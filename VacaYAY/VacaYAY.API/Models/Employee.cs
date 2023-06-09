namespace VacaYAY.API.Models;

public class Employee
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string IdNumber { get; set; }
    public required int DaysOffNumber { get; set; }
    public required Position Position { get; set; }
    public required DateTime EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public required DateTime InsertDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public required List<VacationRequest> VacationRequests { get; set; }
    public required List<VacationReview> VacationReviews { get; set; }
}
