using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class CreateModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public VacationRequest VacationRequest { get; set; } = default!;
    public IList<LeaveType> LeaveTypes { get; set; } = default!;

    [BindProperty]
    public int LeaveTypeId { get; set; } = default;
    public int TotalPotentialDaysOff => PotentialLastYearsDaysOff + PotentialNewDaysOff;
    public required int PotentialLastYearsDaysOff { get; set; } = default!;
    public required int PotentialNewDaysOff { get; set; } = default!;
    public Employee LoggedInEmployee { get; set; } = default!;

    private async Task<StatusCodeResult> Init()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypesAsync();

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        LoggedInEmployee = loggedInEmployee;

        int potentiallyUsedDays = await _unitOfWork.VacationService.GetPotentiallyUsedDaysAsync(LoggedInEmployee.Id);

        int overflow = Math.Max(potentiallyUsedDays - LoggedInEmployee.LastYearsDaysOffNumber, 0);
        PotentialLastYearsDaysOff = Math.Max(LoggedInEmployee.LastYearsDaysOffNumber - potentiallyUsedDays, 0);
        PotentialNewDaysOff = Math.Max(LoggedInEmployee.DaysOffNumber - overflow, 0);

        return StatusCode(StatusCodes.Status200OK);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await Init();
        if (result.StatusCode == StatusCodes.Status401Unauthorized)
        {
            return Unauthorized();
        }

        VacationRequest = new()
        {
            Comment = string.Empty,
            Employee = LoggedInEmployee,
            StartDate = DateTime.Now.Date.AddDays(1),
            EndDate = DateTime.Now.Date.AddDays(6),
            LeaveType = LeaveTypes.First()
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await Init();
        if (result.StatusCode == StatusCodes.Status401Unauthorized)
        {
            return Unauthorized();
        }

        int potentiallyUsedDays = await _unitOfWork.VacationService.GetPotentiallyUsedDaysAsync(LoggedInEmployee.Id);

        VacationRequest.Employee = LoggedInEmployee;
        VacationRequest.LeaveType = LeaveTypes.Single(lt => lt.Id == LeaveTypeId);
        var requestValidationResult = await _unitOfWork.VacationService.CreateVacationRequestAsync(VacationRequest);

        ModelState.Clear();
        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            await Init();
            return Page();
        }

        await _unitOfWork.SaveChangesAsync();

        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        string emailSubject = $"Vacation requested by {VacationRequest.Employee.FirstName} {VacationRequest.Employee.LastName}";
        string emailBody = VacationRequest.ToString();
        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={VacationRequest.Id}"">
Go to details page
</a>
";

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(LoggedInEmployee.Email!, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }
}
