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
    public int PotentialDaysOff { get; set; } = default;

    public async Task<IActionResult> OnGetAsync()
    {
        LeaveTypes = await _vacationService.GetLeaveTypesAsync();

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        int potentiallyUsedDays = await _vacationService.GetPotentiallyUsedDaysAsync(loggedInEmployee.Id);

        PotentialDaysOff = loggedInEmployee.DaysOffNumber - potentiallyUsedDays;

        VacationRequest = new()
        {
            Comment = string.Empty,
            Employee = loggedInEmployee,
            StartDate = DateTime.Now.Date.AddDays(1),
            EndDate = DateTime.Now.Date.AddDays(6),
            LeaveType = LeaveTypes.FirstOrDefault()!
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LeaveTypes = await _vacationService.GetLeaveTypesAsync();

        LeaveType? leaveType = await _vacationService.GetLeaveTypeByIdAsync(LeaveTypeId);

        if (leaveType is null)
        {
            return NotFound();
        }

        VacationRequest.LeaveType = leaveType;

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        int potentiallyUsedDays = await _vacationService.GetPotentiallyUsedDaysAsync(loggedInEmployee.Id);

        PotentialDaysOff = loggedInEmployee.DaysOffNumber - potentiallyUsedDays;

        await _vacationService.CreateVacationRequestAsync(VacationRequest, User);

        return RedirectToPage("./Index");
    }
}
