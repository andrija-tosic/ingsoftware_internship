using IronPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Business.Validators;
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
    public bool IsLoggedInEmployeeAdmin => User.IsInRole(InitialData.AdminRoleName);

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

        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypesAsync();

        var vacationRequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);
        if (vacationRequest is null)
        {
            return NotFound();
        }

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

        // Since vacation review gets deleted when vacation request is updated, if it was approved, return the taken days to the employee.
        if (vacationRequestFromDb.VacationReview is not null && vacationRequestFromDb.VacationReview.Approved)
        {
            vacationRequestFromDb.Employee.LastYearsDaysOffNumber += vacationRequestFromDb.VacationReview.LastYearsDaysTakenOffNumber;
            vacationRequestFromDb.Employee.DaysOffNumber += previousDays - vacationRequestFromDb.VacationReview.LastYearsDaysTakenOffNumber;

            await _unitOfWork.EmployeeService.UpdateAsync(vacationRequestFromDb.Employee);
        }

        vacationRequestFromDb.VacationReview = VacationRequestDTO.VacationReview;

        var loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        IsSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequestFromDb.Employee.Id;

        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypesAsync();

        LeaveType leaveType = LeaveTypes.Single(lt => lt.Id == VacationRequestDTO.LeaveType.Id);

        vacationRequestFromDb.LeaveType = leaveType;

        var vacationRequestDTOValidationResult = await new VacationRequestValidator(_unitOfWork).ValidateAsync(new VacationRequest
        {
            Id = VacationRequestDTO.Id,
            Comment = VacationRequestDTO.Comment,
            Employee = VacationRequestDTO.Employee,
            StartDate = VacationRequestDTO.StartDate,
            EndDate = VacationRequestDTO.EndDate,
            LeaveType = leaveType,
            VacationReview = VacationRequestDTO.VacationReview
        });

        ModelState.Clear();
        if (!vacationRequestDTOValidationResult.IsValid)
        {
            foreach (var error in vacationRequestDTOValidationResult.Errors)
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

        if (IsSameEmployeeAsLoggedInOne)
        {
            vacationRequestFromDb.Comment = VacationRequestDTO.Comment;
            vacationRequestFromDb.StartDate = VacationRequestDTO.StartDate;
            vacationRequestFromDb.EndDate = VacationRequestDTO.EndDate;
        }

        _unitOfWork.VacationService.UpdateVacationRequest(vacationRequestFromDb);

        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={vacationRequestFromDb.Id}"">
Go to details page
</a>
";

        var employeeValidationResult = await _unitOfWork.EmployeeService.UpdateAsync(vacationRequestFromDb.Employee);

        if (!employeeValidationResult.IsValid)
        {
            foreach (var error in employeeValidationResult.Errors)
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

        await _unitOfWork.SaveChangesAsync();

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }
    public async Task<IActionResult> OnPostUpsertVacationRequestReviewAsync()
    {
        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        var vacationRequestFromDb = await _unitOfWork.VacationService.GetVacationRequestByIdAsync(VacationRequestDTO.Id);
        if (vacationRequestFromDb is null)
        {
            return NotFound();
        }

        Employee vacationEmployee = vacationRequestFromDb.Employee;

        VacationReview newVacationreview = VacationRequestDTO.VacationReview;

        newVacationreview.Reviewer = loggedInEmployee;

        string emailBody;
        string emailSubject;

        int days = (vacationRequestFromDb.EndDate.Date - vacationRequestFromDb.StartDate.Date).Days;

        if (newVacationreview.Id == 0) // Create
        {
            newVacationreview.LastYearsDaysTakenOffNumber = Math.Min(vacationEmployee.LastYearsDaysOffNumber, days);

            newVacationreview.VacationRequest = vacationRequestFromDb;
            newVacationreview.VacationRequestRefId = vacationRequestFromDb.Id;

            if (newVacationreview.Approved)
            {
                vacationEmployee.LastYearsDaysOffNumber -= newVacationreview.LastYearsDaysTakenOffNumber;
                vacationEmployee.DaysOffNumber -= days - newVacationreview.LastYearsDaysTakenOffNumber;

                await _unitOfWork.EmployeeService.UpdateAsync(vacationEmployee);

                GenerateAndSendVacationReportEmail(newVacationreview);
            }

            _unitOfWork.VacationService.CreateVacationReview(newVacationreview);

            emailSubject = $"Vacation review created for vacation request from {vacationEmployee.FirstName} {vacationEmployee.LastName}";
            emailBody = $@"
            {vacationRequestFromDb}
            {newVacationreview}
            ";

            emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={newVacationreview.Id}"">
Go to details page
</a>
";
        }
        else // Update
        {
            VacationReview? vacationReviewFromDb = await _unitOfWork.VacationService.GetVacationReviewByIdAsync(newVacationreview.Id);

            if (vacationReviewFromDb is null)
            {
                return NotFound();
            }

            newVacationreview.LastYearsDaysTakenOffNumber = vacationReviewFromDb.LastYearsDaysTakenOffNumber;

            bool reviewBecameApproved = !vacationReviewFromDb.Approved && newVacationreview.Approved;
            bool reviewBecameRejected = vacationReviewFromDb.Approved && !newVacationreview.Approved;


            if (reviewBecameApproved)
            {
                newVacationreview.LastYearsDaysTakenOffNumber = Math.Min(vacationEmployee.LastYearsDaysOffNumber, days);
                
                vacationEmployee.LastYearsDaysOffNumber -= newVacationreview.LastYearsDaysTakenOffNumber;
                vacationEmployee.DaysOffNumber -= days - newVacationreview.LastYearsDaysTakenOffNumber;

                GenerateAndSendVacationReportEmail(vacationReviewFromDb);
            }
            else if (reviewBecameRejected)
            {
                vacationEmployee.LastYearsDaysOffNumber += newVacationreview.LastYearsDaysTakenOffNumber;
                vacationEmployee.DaysOffNumber += days - newVacationreview.LastYearsDaysTakenOffNumber;
            }
            await _unitOfWork.EmployeeService.UpdateAsync(vacationEmployee);

            vacationReviewFromDb.Reviewer = loggedInEmployee;
            vacationReviewFromDb.Approved = newVacationreview.Approved;
            vacationReviewFromDb.Comment = newVacationreview.Comment;

            emailSubject = $"Vacation review updated for vacation request from {vacationEmployee.FirstName} {vacationEmployee.LastName}";
            emailBody = $@"
{vacationRequestFromDb}
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

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteVacationRequestReviewAsync()
    {
        var vacationReview = await _unitOfWork.VacationService.GetVacationReviewByIdAsync(VacationRequestDTO.VacationReview.Id);

        if (vacationReview is null)
        {
            return NotFound();
        }

        int days = (vacationReview.VacationRequest.EndDate.Date - vacationReview.VacationRequest.StartDate.Date).Days;
        var vacationEmployee = vacationReview.VacationRequest.Employee;

        if (vacationReview.Approved)
        {
            vacationEmployee.LastYearsDaysOffNumber += vacationReview.LastYearsDaysTakenOffNumber;
            vacationEmployee.DaysOffNumber += days - vacationReview.LastYearsDaysTakenOffNumber;
        }

        await _unitOfWork.VacationService.DeleteVacationReviewAsync(VacationRequestDTO.VacationReview.Id);

        await _unitOfWork.EmployeeService.UpdateAsync(vacationEmployee);

        await _unitOfWork.SaveChangesAsync();

        string emailSubject = "Vacation review deleted for vacation request";
        string emailBody = $@"
            {vacationReview.VacationRequest}
            {vacationReview}
            ";

        var loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }

    private async Task GenerateAndSendEmailsToHrEmployeesAsync(Employee employee, string emailSubject, string emailBody)
    {
        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(employee.Email!, emailSubject, emailBody);
    }

    private void GenerateAndSendVacationReportEmail(VacationReview vacationReview)
    {
        PdfDocument vacationReportPdf = _unitOfWork.VacationService.GenerateVacationReportPdf(vacationReview);

        string pdfName = $"{vacationReview.VacationRequest.Employee.FirstName}" +
            $"-" +
            $"{vacationReview.VacationRequest.Employee.LastName}" +
            $"-" +
            $"{vacationReview.VacationRequest.StartDate.Date.ToShortDateString()}" +
            $"-" +
            $"{vacationReview.VacationRequest.EndDate.Date.ToShortDateString()}" +
            $".pdf";

        byte[] pdfData = vacationReportPdf.BinaryData;

        _unitOfWork.EmailService.EnqueueEmail(vacationReview.VacationRequest.Employee.Email!,
            $"Vacation report for approved vacation request from {vacationReview.VacationRequest.Employee.FirstName} {vacationReview.VacationRequest.Employee.LastName}",
            "",
            pdfData,
            pdfName);
    }
}
