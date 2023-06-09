using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class CreateModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        VacationRequest = new()
        {
            Comment = string.Empty,
            Employee = loggedInEmployee,
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(8),
            LeaveType = LeaveTypes.FirstOrDefault()!,
        };


        return Page();
    }

    [BindProperty]
    public VacationRequest VacationRequest { get; set; } = default!;

    public IList<LeaveType> LeaveTypes { get; set; } = default!;

    [BindProperty]
    public int LeaveTypeId { get; set; } = default;

    public async Task<IActionResult> OnPostAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        LeaveType? leaveType = await _unitOfWork.VacationService.GetLeaveTypeById(LeaveTypeId);

        if (leaveType is null)
        {
            return NotFound();
        }

        VacationRequest.LeaveType = leaveType;

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        VacationRequest.Employee = loggedInEmployee;
        var requestValidationResult = await _unitOfWork.VacationService.CreateVacationRequest(VacationRequest);

        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return Page();
        }

        int days = (VacationRequest.EndDate - VacationRequest.StartDate).Days;
        VacationRequest.Employee.DaysOffNumber -= days;
        var employeeValidationResult = await _unitOfWork.EmployeeService.UpdateAsync(VacationRequest.Employee);

        if (!employeeValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return Page();

        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
