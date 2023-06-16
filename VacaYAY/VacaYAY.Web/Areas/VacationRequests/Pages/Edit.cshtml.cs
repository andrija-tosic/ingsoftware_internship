using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
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
    public bool IsLoggedInEmployeeAdmin = false;

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
        IsLoggedInEmployeeAdmin = await _unitOfWork.EmployeeService.IsInRoleAsync(loggedInEmployee, InitialData.AdminRoleName);

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

        string emailSubject = $"Vacation request from {VacationRequestDTO.Employee.FirstName} {VacationRequestDTO.Employee.LastName} updated";
        string emailBody = $@"
Old request:
{vacationRequestFromDb}
Updated request:
{VacationRequestDTO}
";

        int previousDays = (vacationRequestFromDb.EndDate.Date - vacationRequestFromDb.StartDate.Date).Days;
        int newDays = (VacationRequestDTO.EndDate.Date - VacationRequestDTO.StartDate.Date).Days;

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

        IsLoggedInEmployeeAdmin = await _unitOfWork.EmployeeService.IsInRoleAsync(loggedInEmployee, InitialData.AdminRoleName);

        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

        LeaveType? leaveType = await _unitOfWork.VacationService.GetLeaveTypeById(VacationRequestDTO.LeaveType.Id);

        if (leaveType is null)
        {
            return NotFound();
        }

        vacationRequestFromDb.LeaveType = leaveType;

        var requestValidationResult = await _unitOfWork.VacationService.UpdateVacationRequest(vacationRequestFromDb);
        
        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={vacationRequestFromDb.Id}"">
Go to details page
</a>
";

        ModelState.Clear();
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

        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }
    public async Task<IActionResult> OnPostUpsertVacationRequestReviewAsync()
    {
        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        VacationReview vacationReviewModel = VacationRequestDTO.VacationReview;

        vacationReviewModel.Reviewer = loggedInEmployee;

        string emailBody;
        string emailSubject;

        if (vacationReviewModel.Id == 0) // Create
        {
            emailSubject = $"Vacation review created for vacation request from {VacationRequestDTO.Employee.FirstName} {VacationRequestDTO.Employee.LastName}";
            emailBody = $@"
            {VacationRequestDTO}
            {VacationRequestDTO.VacationReview}
            ";

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

            emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={vacationReviewModel.Id}"">
Go to details page
</a>
";

        }
        else // Update
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

            emailSubject = $"Vacation review updated for vacation request from {VacationRequestDTO.Employee.FirstName} {VacationRequestDTO.Employee.LastName}";
            emailBody = $@"
            {VacationRequestDTO}
            {vacationReviewFromDb}
            ";

            _unitOfWork.VacationService.UpdateVacationReview(vacationReviewFromDb);

            emailBody += $@"
<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={vacationReviewFromDb.Id}"">
Go to details page
</a>
";
        }

        await _unitOfWork.SaveChangesAsync();

        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteVacationRequestReviewAsync()
    {
        await _unitOfWork.VacationService.DeleteVacationReviewAsync(VacationRequestDTO.VacationReview.Id);

        await _unitOfWork.SaveChangesAsync();

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        string emailSubject = "Vacation review deleted for vacation request";
        string emailBody = $@"
            { VacationRequestDTO}
            {VacationRequestDTO.VacationReview}
            ";

        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }
}
