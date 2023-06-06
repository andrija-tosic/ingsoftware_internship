using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
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

    public async Task<IActionResult> OnGetAsync()
    {
        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        bool isAdmin = await _unitOfWork.EmployeeService.IsInRoleAsync(loggedInEmployee, nameof(UserRoles.Administrator));
        VacationRequests = await _unitOfWork.VacationService.GetAllVacationRequestsAsync(loggedInEmployee.Id, isAdmin);

        return Page();
    }
}
