using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class EditModel : PageModel
{
    private readonly IVacationService _vacationService;
    private readonly IEmployeeService _employeeService;

    public EditModel(
        IVacationService vacationService,
        IEmployeeService employeeService
        )
    {
        _vacationService = vacationService;
        _employeeService = employeeService;
    }

    [BindProperty]
    public VacationRequestDTO VacationRequestDTO { get; set; } = default!;
    public IList<LeaveType> LeaveTypes { get; set; } = default!;
    public bool IsSameEmployeeAsLoggedInOne = false;
    public bool IsLoggedInEmployeeAdmin => User.IsInRole(InitialData.AdminRoleName);

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        var vacationRequest = await _vacationService.GetVacationRequestByIdAsync((int)id);
        if (vacationRequest is null)
        {
            return NotFound();
        }

        IsSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequest.Employee.Id;

        LeaveTypes = await _vacationService.GetLeaveTypesAsync();
        if (!User.IsInRole(InitialData.AdminRoleName))
        {
            LeaveTypes = LeaveTypes.Where(lt => lt.Id != InitialData.CollectiveVacationLeaveType.Id).ToList();
        }

        VacationRequestDTO = new()
        {
            Id = vacationRequest.Id,
            Comment = vacationRequest.Comment,
            VacationReview = vacationRequest.VacationReview is not null
            ? vacationRequest.VacationReview
            : new VacationReview()
            {
                Approved = false,
                Comment = string.Empty,
                Reviewer = default!,
                VacationRequestRefId = vacationRequest.Id,
                VacationRequest = vacationRequest,
                LastYearsDaysTakenOffNumber = 0
            },
            StartDate = vacationRequest.StartDate,
            EndDate = vacationRequest.EndDate,
            Employee = vacationRequest.Employee,
            LeaveType = vacationRequest.LeaveType
        };

        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        VacationRequest? vacationRequestFromDb = await _vacationService.GetVacationRequestByIdAsync(VacationRequestDTO.Id);

        if (vacationRequestFromDb is null)
        {
            return NotFound();
        }

        var loggedInEmployee = await _employeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        IsSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequestFromDb.Employee.Id;

        LeaveTypes = await _vacationService.GetLeaveTypesAsync();
        if (!User.IsInRole(InitialData.AdminRoleName))
        {
            LeaveTypes = LeaveTypes.Where(lt => lt.Id != InitialData.CollectiveVacationLeaveType.Id).ToList();
        }

        var validationResult = await _vacationService.UpdateVacationRequestAsync(VacationRequestDTO, User);

        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                VacationRequestDTO.VacationReview ??= new VacationReview()
                {
                    Approved = false,
                    Comment = string.Empty,
                    Reviewer = default!,
                    VacationRequest = default!,
                    LastYearsDaysTakenOffNumber = 0
                };

                return Page();
            }
        }

        return RedirectToPage("./Index");
    }
    public async Task<IActionResult> OnPostUpsertVacationRequestReviewAsync()
    {
        if (VacationRequestDTO.VacationReview.Id == 0)
        {
            await _vacationService.CreateVacationReviewAsync(VacationRequestDTO.VacationReview, VacationRequestDTO.Id, User);
        }
        else
        {
            await _vacationService.UpdateVacationReviewAsync(VacationRequestDTO.VacationReview, VacationRequestDTO.Id, User);
        }

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteVacationRequestReviewAsync()
    {
        await _vacationService.DeleteVacationReviewAsync(VacationRequestDTO.VacationReview.Id, User);

        return RedirectToPage("./Index");
    }
}
