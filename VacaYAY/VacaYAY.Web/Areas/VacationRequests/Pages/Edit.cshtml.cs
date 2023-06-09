using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class EditModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public EditModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public VacationRequestDTO VacationRequestDTO { get; set; } = default!;

    public IList<LeaveType> LeaveTypes { get; set; } = default!;
    public bool IsSameEmployeeAsLoggedInOne = false;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        var vacationRequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);
        if (vacationRequest is null)
        {
            return NotFound();
        }

        TempData["vacationRequestId"] = vacationRequest.Id;

        IsSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequest.Employee.Id;

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
                VacationRequest = vacationRequest
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
        VacationRequest? vacationRequestFromDb = await _unitOfWork.VacationService.GetVacationRequestByIdAsync(VacationRequestDTO.Id);

        if (vacationRequestFromDb is null)
        {
            return NotFound();
        }

        VacationRequestDTO.Employee = vacationRequestFromDb.Employee;

        int previousDays = (vacationRequestFromDb.EndDate - vacationRequestFromDb.StartDate).Days;
        int newDays = (VacationRequestDTO.EndDate - VacationRequestDTO.StartDate).Days;

        vacationRequestFromDb.VacationReview = VacationRequestDTO.VacationReview;

        var loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        IsSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequestFromDb.Employee.Id;
        
        if (IsSameEmployeeAsLoggedInOne)
        {
            vacationRequestFromDb.Comment = VacationRequestDTO.Comment;
            vacationRequestFromDb.StartDate = VacationRequestDTO.StartDate;
            vacationRequestFromDb.EndDate = VacationRequestDTO.EndDate;
        }

        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        LeaveType? leaveType = await _unitOfWork.VacationService.GetLeaveTypeById(VacationRequestDTO.LeaveType.Id);

        if (leaveType is null)
        {
            return NotFound();
        }

        vacationRequestFromDb.LeaveType = leaveType;

        var requestValidationResult = await _unitOfWork.VacationService.UpdateVacationRequest(vacationRequestFromDb);

        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            VacationRequestDTO.VacationReview ??= new VacationReview()
            {
                Approved = false,
                Comment = string.Empty,
                Reviewer = default!,
                VacationRequest = default!
            };

            return Page();
        }

        vacationRequestFromDb.Employee.DaysOffNumber += previousDays - newDays;
        var employeeValidationResult = await _unitOfWork.EmployeeService.UpdateAsync(vacationRequestFromDb.Employee);

        if (!employeeValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            VacationRequestDTO.VacationReview ??= new VacationReview()
            {
                Approved = false,
                Comment = string.Empty,
                Reviewer = default!,
                VacationRequest = default!
            };

            return Page();
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostUpsertVacationRequestReviewAsync(VacationRequestDTO requestDTO)
    {
        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        VacationReview vacationReviewModel = VacationRequestDTO.VacationReview;

        vacationReviewModel.Reviewer = loggedInEmployee;

        if (vacationReviewModel.Id == 0)
        {
            int vacationRequestId = (int)TempData["vacationRequestId"]!;

            var vacationRequestFromDb = await _unitOfWork.VacationService.GetVacationRequestByIdAsync(vacationRequestId);

            if (vacationRequestFromDb is null)
            {
                return NotFound();
            }

            vacationReviewModel.VacationRequest = vacationRequestFromDb;
            vacationReviewModel.VacationRequestRefId = vacationRequestFromDb.Id;

            if (vacationReviewModel.Approved)
            {
                loggedInEmployee.DaysOffNumber -= (VacationRequestDTO.EndDate - VacationRequestDTO.StartDate).Days;

                await _unitOfWork.EmployeeService.UpdateAsync(loggedInEmployee);
            }

            _unitOfWork.VacationService.CreateVacationReview(vacationReviewModel);
        }
        else
        {
            VacationReview? vacationReviewFromDb = await _unitOfWork.VacationService.GetVacationReviewByIdAsync(vacationReviewModel.Id);

            if (vacationReviewFromDb is null)
            {
                return NotFound();
            }

            bool reviewBecameApproved = !vacationReviewFromDb.Approved && vacationReviewModel.Approved;
            bool reviewBecameRejected = vacationReviewFromDb.Approved && !vacationReviewModel.Approved;

            if (reviewBecameApproved)
            {
                loggedInEmployee.DaysOffNumber -= (VacationRequestDTO.EndDate - VacationRequestDTO.StartDate).Days;
                await _unitOfWork.EmployeeService.UpdateAsync(loggedInEmployee);
            }
            else if (reviewBecameRejected)
            {
                loggedInEmployee.DaysOffNumber += (VacationRequestDTO.EndDate - VacationRequestDTO.StartDate).Days;
                await _unitOfWork.EmployeeService.UpdateAsync(loggedInEmployee);
            }

            vacationReviewFromDb.Reviewer = loggedInEmployee;
            vacationReviewFromDb.Approved = vacationReviewModel.Approved;
            vacationReviewFromDb.Comment = vacationReviewModel.Comment;

            _unitOfWork.VacationService.UpdateVacationReview(vacationReviewFromDb);
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteVacationRequestReviewAsync()
    {
        await _unitOfWork.VacationService.DeleteVacationReviewAsync(VacationRequestDTO.VacationReview.Id);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
        }
        return RedirectToPage("./Index");
    }
}
