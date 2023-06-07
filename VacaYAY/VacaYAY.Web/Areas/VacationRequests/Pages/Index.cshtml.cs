using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IList<VacationRequest> VacationRequests { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public VacationRequestSearchFilters Input { get; set; } = default!;
    public IList<LeaveType> LeaveTypes { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        bool isAdmin = await _unitOfWork.EmployeeService.IsInRoleAsync(loggedInEmployee, nameof(UserRoles.Administrator));
        VacationRequests = await _unitOfWork.VacationService.SearchVacationRequestsAsync(loggedInEmployee.Id, isAdmin, Input);

        return Page();
    }

    public async Task<ActionResult> OnPostSearchAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        bool isAdmin = await _unitOfWork.EmployeeService.IsInRoleAsync(loggedInEmployee, nameof(UserRoles.Administrator));
        VacationRequests = await _unitOfWork.VacationService.SearchVacationRequestsAsync(loggedInEmployee.Id, isAdmin, Input);
        
        return Page();
    }
}
