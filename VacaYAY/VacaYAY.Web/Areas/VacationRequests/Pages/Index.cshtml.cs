using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class IndexModel : PageModel
{
    private readonly IVacationService _vacationService;
    private readonly IEmployeeService _employeeService;

    public IndexModel(
        IVacationService vacationService,
        IEmployeeService employeeService
        )
    {
        _vacationService = vacationService;
        _employeeService = employeeService;
    }

    public IList<VacationRequest> VacationRequests { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public VacationRequestSearchFilters Input { get; set; } = default!;
    public IList<LeaveType> LeaveTypes { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        LeaveTypes = await _vacationService.GetLeaveTypesAsync();

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        bool isAdmin = await _employeeService.IsInRoleAsync(loggedInEmployee, InitialData.AdminRoleName);
        VacationRequests = await _vacationService.SearchVacationRequestsAsync(loggedInEmployee.Id, isAdmin, Input);

        return Page();
    }

    public async Task<ActionResult> OnPostSearchAsync()
    {
        LeaveTypes = await _vacationService.GetLeaveTypesAsync();

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        bool isAdmin = await _employeeService.IsInRoleAsync(loggedInEmployee, InitialData.AdminRoleName);
        VacationRequests = await _vacationService.SearchVacationRequestsAsync(loggedInEmployee.Id, isAdmin, Input);
        
        return Page();
    }
}
