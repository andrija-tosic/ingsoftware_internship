using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class CreateModel : PageModel
{
    private readonly IVacationService _vacationService;
    private readonly IEmployeeService _employeeService;

    public CreateModel(
        IVacationService vacationService,
        IEmployeeService employeeService
        )
    {
        _vacationService = vacationService;
        _employeeService = employeeService;
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
        LeaveTypes = await _vacationService.GetLeaveTypesAsync();

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        LoggedInEmployee = loggedInEmployee;

        int potentiallyUsedDays = await _vacationService.GetPotentiallyUsedDaysAsync(LoggedInEmployee.Id);

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

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        VacationRequest.Employee = LoggedInEmployee;
        VacationRequest.LeaveType = LeaveTypes.Single(lt => lt.Id == LeaveTypeId);

        var requestValidationResult = await _vacationService.CreateVacationRequestAsync(VacationRequest, User);

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

        return RedirectToPage("./Index");
    }
}
