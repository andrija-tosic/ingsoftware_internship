using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VacaYAY.Business.Validators;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace VacaYAY.Business.Services;

public class VacationService : IVacationService
{
    private readonly VacayayDbContext _context;
    private readonly IValidator<VacationRequest> _vacationRequestValidator;
    private readonly IEmployeeService _employeeService;
    private readonly IEmailService _emailService;
    private readonly ILogger<IVacationService> _logger;

    public VacationService(
        VacayayDbContext context,
        IEmployeeService employeeService,
        IEmailService emailService,
        ILogger<IVacationService> logger)
    {
        _context = context;
        _vacationRequestValidator = new VacationRequestValidator(this);
        _employeeService = employeeService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<VacationRequest?> GetVacationRequestByIdAsync(int id)
    {
        return await _context.VacationRequests
            .Where(v => v.Id == id)
            .Include(v => v.LeaveType)
            .Include(v => v.Employee).ThenInclude(e => e.Position)
            .Include(v => v.VacationReview)
            .SingleAsync();
    }

    public async Task<ValidationResult> CreateVacationRequestAsync(VacationRequest vacationRequest, ClaimsPrincipal AuthenticatedUser)
    {
        int days = (vacationRequest.EndDate - vacationRequest.StartDate).Days;

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        //TODO
        //if (loggedInEmployee is null)
        //{
        //    return NotFound();
        //}

        int potentiallyUsedDays = await GetPotentiallyUsedDaysAsync(loggedInEmployee.Id);

        vacationRequest.Employee = loggedInEmployee;
        var validationResult = await _vacationRequestValidator.ValidateAsync(vacationRequest);

        if (!validationResult.IsValid)
        {
            return validationResult;
        }
        _context.VacationRequests.Add(vacationRequest);
        await _context.SaveChangesAsync();

        if (vacationRequest.LeaveType.Id == InitialData.CollectiveVacationLeaveType.Id
                && AuthenticatedUser.IsInRole(InitialData.AdminRoleName))
        {
            await _employeeService.SubtractDaysFromAllEmployees(days);

            var employees = await _employeeService.GetAllAsync();

            foreach (var employee in employees)
            {
                string subject = $"Collective vacation requested by {vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                string body = vacationRequest.ToString();
                body += $@"
Your remaining number of days off: {employee.DaysOffNumber}
Your remaining number of old days off: {employee.LastYearsDaysOffNumber}

<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationRequest.Id}"">
Go to details page
</a>
";

                _emailService.EnqueueEmail(employee.Email!, subject, body);
            }

            return validationResult;
        }

        string emailSubject = $"Vacation requested by {vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
        string emailBody = vacationRequest.ToString();
        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationRequest.Id}"">
Go to details page
</a>
";
        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);

        return validationResult;
    }
    public async Task<ValidationResult> UpdateVacationRequestAsync(
        VacationRequestDTO vacationRequestDto,
        ClaimsPrincipal AuthenticatedUser
        )
    {
        VacationRequest? vacationRequestFromDb = await GetVacationRequestByIdAsync(vacationRequestDto.Id);

        //TODO
        //if (vacationRequestFromDb is null)
        //{
        //    return NotFound();
        //}

        vacationRequestDto.Employee = vacationRequestFromDb.Employee;
        var validationResult = await _vacationRequestValidator.ValidateAsync(new VacationRequest
        {
            Id = vacationRequestDto.Id,
            Employee = vacationRequestDto.Employee,
            Comment = vacationRequestDto.Comment,
            LeaveType = vacationRequestDto.LeaveType,
            StartDate = vacationRequestDto.StartDate,
            EndDate = vacationRequestDto.EndDate,
            VacationReview = vacationRequestDto.VacationReview
        });

        if (!validationResult.IsValid)
        {
            return validationResult;
        }

        string emailSubject = $"Vacation request from {vacationRequestDto.Employee.FirstName} {vacationRequestDto.Employee.LastName} updated";
        string emailBody = $@"
Old request:
{vacationRequestFromDb}
Updated request:
{vacationRequestDto}
";

        var vacationRequestValidator = new VacationRequestValidator(this);

        int previousDays = (vacationRequestFromDb.EndDate.Date - vacationRequestFromDb.StartDate.Date).Days;
        int newDays = (vacationRequestDto.EndDate.Date - vacationRequestDto.StartDate.Date).Days;

        // Since vacation review gets deleted when vacation request is updated, if it was approved, return the taken days to the employee.
        if (vacationRequestFromDb.VacationReview is not null && vacationRequestFromDb.VacationReview.Approved)
        {
            vacationRequestFromDb.Employee.LastYearsDaysOffNumber += vacationRequestFromDb.VacationReview.LastYearsDaysTakenOffNumber;
            vacationRequestFromDb.Employee.DaysOffNumber += previousDays - vacationRequestFromDb.VacationReview.LastYearsDaysTakenOffNumber;

            var employeeValidationResult = await _employeeService.UpdateAsync(vacationRequestFromDb.Employee);

            if (!employeeValidationResult.IsValid)
            {
                return employeeValidationResult;
            }
        }

        vacationRequestFromDb.VacationReview = vacationRequestDto.VacationReview;

        var loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        // TODO
        //if (loggedInEmployee is null)
        //{
        //    return Unauthorized();
        //}

        bool isSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequestFromDb.Employee.Id;

        LeaveType? leaveType = await GetLeaveTypeByIdAsync(vacationRequestDto.LeaveType.Id);

        // TODO
        //if (leaveType is null)
        //{
        //    return NotFound();
        //}

        vacationRequestFromDb.LeaveType = leaveType;

        var vacationRequestDTOValidationResult = await vacationRequestValidator.ValidateAsync(new VacationRequest
        {
            Id = vacationRequestDto.Id,
            Comment = vacationRequestDto.Comment,
            Employee = vacationRequestDto.Employee,
            StartDate = vacationRequestDto.StartDate,
            EndDate = vacationRequestDto.EndDate,
            LeaveType = leaveType,
            VacationReview = vacationRequestDto.VacationReview
        });

        if (!vacationRequestDTOValidationResult.IsValid)
        {
            return vacationRequestDTOValidationResult;
        }

        if (isSameEmployeeAsLoggedInOne)
        {
            vacationRequestFromDb.Comment = vacationRequestDto.Comment;
            vacationRequestFromDb.StartDate = vacationRequestDto.StartDate;
            vacationRequestFromDb.EndDate = vacationRequestDto.EndDate;
        }

        _context.VacationRequests.Update(vacationRequestFromDb);
        await _context.SaveChangesAsync();

        emailBody += $@"
<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationRequestFromDb.Id}"">
Go to details page
</a>
";

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);

        return new ValidationResult();
    }

    public async Task DeleteVacationRequestAsync(int id, ClaimsPrincipal AuthenticatedUser)
    {
        var loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        //TODO
        //if (loggedInEmployee is null)
        //{
        //    return Unauthorized();
        //}

        var vacationRequest = await GetVacationRequestByIdAsync(id);

        //TODO
        //if (vacationrequest is null)
        //{
        //    return NotFound();
        //}

        if (vacationRequest.VacationReview is not null && vacationRequest.VacationReview.Approved)
        {
            int days = (vacationRequest.EndDate - vacationRequest.StartDate).Days;
            vacationRequest.Employee.LastYearsDaysOffNumber += vacationRequest.VacationReview.LastYearsDaysTakenOffNumber;
            vacationRequest.Employee.DaysOffNumber += days - vacationRequest.VacationReview.LastYearsDaysTakenOffNumber;
        }

        _context.Remove(vacationRequest);
        await _employeeService.UpdateAsync(vacationRequest.Employee);

        string emailSubject = $"Vacation request from {vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName} deleted";
        string emailBody = vacationRequest.ToString();

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);
    }

    public async Task<IList<VacationRequest>> SearchVacationRequestsAsync(string employeeId, bool isAdmin, VacationRequestSearchFilters searchFilters)
    {
        IQueryable<VacationRequest> vacationRequests = _context.VacationRequests
            .Include(v => v.LeaveType)
            .Include(v => v.VacationReview)
            .Include(v => v.Employee)
            .AsQueryable();

        if (!isAdmin)
        {
            // Return only this employee's vacation requests if they don't have admin privileges.
            // Otherwise, if admin, return all.

            vacationRequests = vacationRequests.Where(v => v.Employee.Id == employeeId);
        }

        if (searchFilters.LeaveTypeId is not null)
        {
            vacationRequests = vacationRequests.Where(v => v.LeaveType.Id == searchFilters.LeaveTypeId);
        }

        if (searchFilters.StartDate is not null)
        {
            vacationRequests = vacationRequests.Where(v => v.StartDate <= searchFilters.StartDate);
        }

        if (searchFilters.EndDate is not null)
        {
            vacationRequests = vacationRequests.Where(v => v.EndDate >= searchFilters.EndDate);
        }

        IQueryable<VacationRequest> employees = vacationRequests.Where(v => false);

        if (!string.IsNullOrWhiteSpace(searchFilters.EmployeeFullName))
        {
            foreach (string token in searchFilters.EmployeeFullName!.Trim().Split(" "))
            {
                employees = employees.Union(vacationRequests.Where(v => v.Employee.FirstName.Contains(token) || v.Employee.LastName.Contains(token)));
            }
        }

        if (employees.Any())
        {
            return await vacationRequests.Intersect(employees).ToListAsync();
        }
        else
        {
            return await vacationRequests.ToListAsync();
        }
    }

    public async Task<IList<LeaveType>> GetLeaveTypesAsync()
    {
        return await _context.LeaveTypes.ToListAsync();
    }

    public async Task<LeaveType?> GetLeaveTypeByIdAsync(int id)
    {
        return await _context.LeaveTypes.FindAsync(id);
    }

    public async Task CreateVacationReviewAsync(
        VacationReview vacationReview,
        int vacationRequestId,
        ClaimsPrincipal AuthenticatedUser
        )
    {
        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        //TODO
        //if (loggedInEmployee is null)
        //{
        //    return NotFound();
        //}

        vacationReview.Reviewer = loggedInEmployee;

        var vacationRequestFromDb = await GetVacationRequestByIdAsync(vacationRequestId);

        //TODO
        //if (vacationRequestFromDb is null)
        //{
        //    return NotFound();
        //}

        int days = (vacationRequestFromDb.EndDate.Date - vacationRequestFromDb.StartDate.Date).Days;

        vacationReview.LastYearsDaysTakenOffNumber = Math.Min(vacationRequestFromDb.Employee.LastYearsDaysOffNumber, days);

        vacationReview.VacationRequest = vacationRequestFromDb;
        vacationReview.VacationRequestRefId = vacationRequestFromDb.Id;

        if (vacationReview.Approved)
        {
            vacationRequestFromDb.Employee.LastYearsDaysOffNumber -= vacationReview.LastYearsDaysTakenOffNumber;
            vacationRequestFromDb.Employee.DaysOffNumber -= days - vacationReview.LastYearsDaysTakenOffNumber;

            await _employeeService.UpdateAsync(vacationRequestFromDb.Employee);

            var pdf = GenerateVacationReportPdf(vacationReview);
            SendVacationReportEmail(vacationReview, pdf);
        }

        _context.VacationReviews.Add(vacationReview);
        await _context.SaveChangesAsync();

        string emailSubject = $"Vacation review created for vacation request from {vacationRequestFromDb.Employee.FirstName} {vacationRequestFromDb.Employee.LastName}";
        string emailBody = $@"
            {vacationRequestFromDb}
            {vacationReview}
            ";

        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationReview.Id}"">
Go to details page
</a>
";

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);
    }

    public async Task UpdateVacationReviewAsync(
        VacationReview vacationReview,
        int vacationRequestId,
        ClaimsPrincipal AuthenticatedUser
)
    {
        VacationReview? vacationReviewFromDb = await GetVacationReviewByIdAsync(vacationReview.Id);

        //TODO
        //if (vacationReviewFromDb is null)
        //{
        //    return NotFound();
        //}

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        //TODO
        //if (loggedInEmployee is null)
        //{
        //    return NotFound();
        //}


        var vacationRequestFromDb = await GetVacationRequestByIdAsync(vacationRequestId);

        //TODO
        //if (vacationRequestFromDb is null)
        //{
        //    return NotFound();
        //}

        vacationReview.LastYearsDaysTakenOffNumber = vacationReviewFromDb.LastYearsDaysTakenOffNumber;

        vacationReview.Reviewer = loggedInEmployee;

        bool reviewBecameApproved = !vacationReviewFromDb.Approved && vacationReview.Approved;
        bool reviewBecameRejected = vacationReviewFromDb.Approved && !vacationReview.Approved;

        int days = (vacationRequestFromDb.EndDate.Date - vacationRequestFromDb.StartDate.Date).Days;

        if (reviewBecameApproved)
        {
            vacationReview.LastYearsDaysTakenOffNumber = Math.Min(vacationRequestFromDb.Employee.LastYearsDaysOffNumber, days);

            vacationRequestFromDb.Employee.LastYearsDaysOffNumber -= vacationReview.LastYearsDaysTakenOffNumber;
            vacationRequestFromDb.Employee.DaysOffNumber -= days - vacationReview.LastYearsDaysTakenOffNumber;

            var pdf = GenerateVacationReportPdf(vacationReview);
            SendVacationReportEmail(vacationReview, pdf);
        }
        else if (reviewBecameRejected)
        {
            vacationRequestFromDb.Employee.LastYearsDaysOffNumber += vacationReview.LastYearsDaysTakenOffNumber;
            vacationRequestFromDb.Employee.DaysOffNumber += days - vacationReview.LastYearsDaysTakenOffNumber;
        }

        vacationReviewFromDb.Reviewer = loggedInEmployee;
        vacationReviewFromDb.Approved = vacationReview.Approved;
        vacationReviewFromDb.Comment = vacationReview.Comment;

        _context.VacationReviews.Update(vacationReviewFromDb);
        await _employeeService.UpdateAsync(vacationRequestFromDb.Employee);

        string emailSubject = $"Vacation review updated for vacation request from {vacationRequestFromDb.Employee.FirstName} {vacationRequestFromDb.Employee.LastName}";
        string emailBody = $@"
{vacationRequestFromDb}
{vacationReviewFromDb}
";

        emailBody += $@"
<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationReviewFromDb.Id}"">
Go to details page
</a>
";
        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);
    }

    public async Task DeleteVacationReviewAsync(int id, ClaimsPrincipal AuthenticatedUser)
    {
        var vacationReview = await GetVacationReviewByIdAsync(id);

        // TODO
        //if (vacationReview is null)
        //{
        //    return NotFound();
        //}

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        // TODO
        //if (loggedInEmployee is null)
        //{
        //    return NotFound();
        //}

        int days = (vacationReview.VacationRequest.EndDate.Date - vacationReview.VacationRequest.StartDate.Date).Days;

        if (vacationReview.Approved)
        {
            vacationReview.VacationRequest.Employee.LastYearsDaysOffNumber += vacationReview.LastYearsDaysTakenOffNumber;
            vacationReview.VacationRequest.Employee.DaysOffNumber += days - vacationReview.LastYearsDaysTakenOffNumber;
        }
        _context.VacationReviews.Remove(vacationReview);
        await _employeeService.UpdateAsync(vacationReview.VacationRequest.Employee);


        string emailSubject = "Vacation review deleted for vacation request";
        string emailBody = $@"
            {vacationReview.VacationRequest}
            {vacationReview}
            ";

        await GenerateAndSendEmailsToHrEmployeesAsync(loggedInEmployee, emailSubject, emailBody);
    }

    public async Task<VacationReview?> GetVacationReviewByIdAsync(int id)
    {
        return await _context.VacationReviews.Where(v => v.Id == id)
            .Include(v => v.VacationRequest).ThenInclude(vr => vr.LeaveType)
            .Include(v => v.VacationRequest).ThenInclude(vr => vr.Employee).ThenInclude(e => e.Position)
            .SingleAsync();
    }

    public PdfDocument GenerateVacationReportPdf(VacationReview vacationReview)
    {
        var document = new HtmlToPdf();

        string htmlContent = $@"
            <h1>Vacation Report</h1>
            <p><strong>Employee:</strong> {vacationReview.VacationRequest.Employee.FirstName} {vacationReview.VacationRequest.Employee.LastName}</p>
            <p><strong>Period:</strong> {vacationReview.VacationRequest.StartDate.ToShortDateString()} - {vacationReview.VacationRequest.EndDate.ToShortDateString()}</p>
            <p><strong>Comment:</strong> {vacationReview.VacationRequest.Comment}</p>
            <p><strong>Leave Type:</strong> {vacationReview.VacationRequest.LeaveType.Name}</p>
            <p><strong>Remaining Days Off:</strong> {vacationReview.VacationRequest.Employee.DaysOffNumber}</p>
            <p><strong>Approved by:</strong> {vacationReview.Reviewer.FirstName} {vacationReview.Reviewer.LastName}</p>
        ";

        PdfDocument pdf = document.RenderHtmlAsPdf(htmlContent);

        return pdf;
    }

    public void SendVacationReportEmail(VacationReview vacationReview, PdfDocument vacationReportPdf)
    {
        string pdfName = $"{vacationReview.VacationRequest.Employee.FirstName}" +
            $"-" +
            $"{vacationReview.VacationRequest.Employee.LastName}" +
            $"-" +
            $"{vacationReview.VacationRequest.StartDate.Date.ToShortDateString()}" +
            $"-" +
            $"{vacationReview.VacationRequest.EndDate.Date.ToShortDateString()}" +
            $".pdf";

        byte[] pdfData = vacationReportPdf.BinaryData;

        _emailService.EnqueueEmail(vacationReview.VacationRequest.Employee.Email!,
            $"Vacation report for approved vacation request from {vacationReview.VacationRequest.Employee.FirstName} {vacationReview.VacationRequest.Employee.LastName}",
            "",
            pdfData,
            pdfName);
    }

    public async Task<int> GetPotentiallyUsedDaysAsync(string employeeId)
    {
        return await _context.VacationRequests
            .Where(v => (v.VacationReview == null)
                     && v.Employee.Id == employeeId)
            .SumAsync(v => EF.Functions.DateDiffDay(v.StartDate, v.EndDate));
    }

    private async Task GenerateAndSendEmailsToHrEmployeesAsync(Employee? loggedInEmployee, string emailSubject, string emailBody)
    {
        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);
    }
}
