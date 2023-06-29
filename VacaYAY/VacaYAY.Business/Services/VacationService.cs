using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace VacaYAY.Business.Services;

public class VacationService : IVacationService
{
    private readonly VacayayDbContext _context;
    private readonly IValidator<VacationRequest> _vacationRequestValidator;
    private readonly ILogger<IVacationService> _logger;

    public VacationService(VacayayDbContext context, IValidator<VacationRequest> vacationRequestValidator, ILogger<IVacationService> logger)
    {
        _context = context;
        _vacationRequestValidator = vacationRequestValidator;
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

    public async Task<ValidationResult> CreateVacationRequestAsync(VacationRequest vacationRequest)
    {
        var validationResult = await _vacationRequestValidator.ValidateAsync(vacationRequest);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return validationResult;
        }

        _context.VacationRequests.Add(vacationRequest);
        return validationResult;
    }
    public void UpdateVacationRequest(VacationRequest vacationRequest)
    {
        _context.VacationRequests.Update(vacationRequest);
    }

    public async Task DeleteVacationRequestAsync(int id)
    {
        var vacationRequest = await _context.VacationRequests
            .Where(v => v.Id == id)
            .Include(v => v.VacationReview)
            .SingleAsync();

        if (vacationRequest is null)
        {
            return;
        }

        _context.VacationRequests.Remove(vacationRequest);
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

    public void CreateVacationReview(VacationReview vacationRequestReview)
    {
        _context.VacationReviews.Add(vacationRequestReview);
    }

    public void UpdateVacationReview(VacationReview vacationRequestReview)
    {
        _context.VacationReviews.Update(vacationRequestReview);
    }

    public async Task DeleteVacationReviewAsync(int id)
    {
        VacationReview? review = await _context.VacationReviews.FindAsync(id);

        if (review is null)
        {
            return;
        }

        _context.VacationReviews.Remove(review);
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
    public async Task<int> GetPotentiallyUsedDaysAsync(string employeeId)
    {
        return await _context.VacationRequests
            .Where(v => v.VacationReview == null
                     && v.Employee.Id == employeeId)
            .SumAsync(v => EF.Functions.DateDiffDay(v.StartDate, v.EndDate));
    }
}
