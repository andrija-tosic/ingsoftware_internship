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

        int days = (vacationRequest.EndDate - vacationRequest.StartDate).Days;

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        string emailSubject = $"Vacation requested by {vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
        string emailBody = vacationRequest.ToString();
        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequest)+"s"}/Details?id={vacationRequest.Id}"">
Go to details page
</a>
";
        
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

        vacationRequestFromDb.VacationReview = vacationRequestDto.VacationReview;

        var loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);


        // TODO
        //if (loggedInEmployee is null)
        //{
        //    return Unauthorized();
        //}

        bool isSameEmployeeAsLoggedInOne = loggedInEmployee.Id == vacationRequestFromDb.Employee.Id;

        bool isLoggedInEmployeeAdmin = await _employeeService.IsInRoleAsync(loggedInEmployee, InitialData.AdminRoleName);

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

        vacationRequestFromDb.Employee.DaysOffNumber += previousDays - newDays;
        var employeeValidationResult = await _employeeService.UpdateAsync(vacationRequestFromDb.Employee);

        if (!employeeValidationResult.IsValid)
        {
            return employeeValidationResult;
        }
        await _context.SaveChangesAsync();

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

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

        var vacationRequest = await GetVacationRequestByIdAsync((int)id);

        //TODO
        //if (vacationrequest is null)
        //{
        //    return NotFound();
        //}

        _context.Remove(vacationRequest);
        await _context.SaveChangesAsync();

        int days = (vacationRequest.EndDate - vacationRequest.StartDate).Days;
        vacationRequest.Employee.DaysOffNumber += days;
        await _employeeService.UpdateAsync(vacationRequest.Employee);

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        string emailSubject = $"Vacation request from {vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName} deleted";
        string emailBody = vacationRequest.ToString();

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);
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

        vacationReview.VacationRequest = vacationRequestFromDb;
        vacationReview.VacationRequestRefId = vacationRequestFromDb.Id;

        if (vacationReview.Approved)
        {
            vacationRequestFromDb.Employee.DaysOffNumber -= (vacationRequestFromDb.EndDate - vacationRequestFromDb.StartDate).Days;

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

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);
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

        vacationReview.Reviewer = loggedInEmployee;

        var vacationRequestFromDb = await GetVacationRequestByIdAsync(vacationRequestId);

        //TODO
        //if (vacationRequestFromDb is null)
        //{
        //    return NotFound();
        //}

        vacationReview.VacationRequest = vacationRequestFromDb;
        vacationReview.VacationRequestRefId = vacationRequestFromDb.Id;

        bool reviewBecameApproved = !vacationReviewFromDb.Approved && vacationReview.Approved;
        bool reviewBecameRejected = vacationReviewFromDb.Approved && !vacationReview.Approved;

        if (reviewBecameApproved)
        {
            vacationRequestFromDb.Employee.DaysOffNumber -= (vacationRequestFromDb.EndDate - vacationRequestFromDb.StartDate).Days;

            var pdf = GenerateVacationReportPdf(vacationReview);
            SendVacationReportEmail(vacationReview, pdf);
        }
        else if (reviewBecameRejected)
        {
            vacationRequestFromDb.Employee.DaysOffNumber += (vacationRequestFromDb.EndDate - vacationRequestFromDb.StartDate).Days;
        }
        await _employeeService.UpdateAsync(vacationRequestFromDb.Employee);

        vacationReviewFromDb.Reviewer = loggedInEmployee;
        vacationReviewFromDb.Approved = vacationReview.Approved;
        vacationReviewFromDb.Comment = vacationReview.Comment;

        string emailSubject = $"Vacation review updated for vacation request from {vacationRequestFromDb.Employee.FirstName} {vacationRequestFromDb.Employee.LastName}";
        string emailBody = $@"
{vacationRequestFromDb}
{vacationReviewFromDb}
";

        _context.VacationReviews.Update(vacationReviewFromDb);
        await _context.SaveChangesAsync();


        emailBody += $@"
<a href=""https://localhost:7085/{nameof(VacationRequest) + "s"}/Details?id={vacationReviewFromDb.Id}"">
Go to details page
</a>
";

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);
    }

    public async Task DeleteVacationReviewAsync(int id, ClaimsPrincipal AuthenticatedUser)
    {
        var vacationReview = await _context.VacationReviews.FindAsync(id);

        // TODO
        //if (vacationReview is null)
        //{
        //    return NotFound();
        //}

        _context.VacationReviews.Remove(vacationReview);
        await _context.SaveChangesAsync();

        Employee? loggedInEmployee = await _employeeService.GetLoggedInAsync(AuthenticatedUser);

        // TODO
        //if (loggedInEmployee is null)
        //{
        //    return NotFound();
        //}

        string emailSubject = "Vacation review deleted for vacation request";
        string emailBody = $@"
            {vacationReview.VacationRequest}
            {vacationReview}
            ";

        var hrEmployees = await _employeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        foreach (var e in hrEmployees)
        {
            _emailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _emailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);
    }

    public async Task<VacationReview?> GetVacationReviewByIdAsync(int id)
    {
        return await _context.VacationReviews.Where(v => v.Id == id)
            .Include(v => v.VacationRequest).ThenInclude(vr => vr.LeaveType)
            .Include(v => v.VacationRequest).ThenInclude(vr => vr.Employee)
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
            .Where(v => v.VacationReview == null
                     && v.Employee.Id == employeeId)
            .SumAsync(v => EF.Functions.DateDiffDay(v.StartDate, v.EndDate));
    }
}
