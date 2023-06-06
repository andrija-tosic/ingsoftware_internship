using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacaYAY.Data;
using VacaYAY.Data.Models;

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
            .Include(v => v.Employee)
            .Include(v => v.VacationReview)
            .SingleAsync();
    }

    public async Task CreateVacationRequest(VacationRequest vacationRequest)
    {
        var validationResult = await _vacationRequestValidator.ValidateAsync(vacationRequest);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return;
        }

        _context.VacationRequests.Add(vacationRequest);
    }
    public async Task UpdateVacationRequest(VacationRequest vacationRequest)
    {
        var validationResult = await _vacationRequestValidator.ValidateAsync(vacationRequest);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return;
        }

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

    public async Task<IList<VacationRequest>> GetAllVacationRequestsAsync(string employeeId, bool isAdmin)
    {
        IQueryable<VacationRequest> vacationRequests = _context.VacationRequests
            .Include(v => v.LeaveType)
            .Include(v => v.VacationReview)
            .Include(v => v.Employee)
            .AsQueryable();

        if (!isAdmin) // Return only this employee's vacation requests if they don't have admin privileges.
        {
            vacationRequests = vacationRequests.Where(v => v.Employee.Id == employeeId);
        }

        return await vacationRequests.ToListAsync();
    }

    public async Task<IList<LeaveType>> GetLeaveTypes()
    {
        return await _context.LeaveTypes.ToListAsync();
    }

    public async Task<LeaveType?> GetLeaveTypeById(int id)
    {
        return await _context.LeaveTypes.FindAsync(id);
    }

    public void CreateVacationRequestReview(VacationRequestReview vacationRequestReview)
    {
        _context.VacationRequestsReviews.Add(vacationRequestReview);
    }

    public void UpdateVacationRequestReview(VacationRequestReview vacationRequestReview)
    {
        _context.VacationRequestsReviews.Update(vacationRequestReview);
    }

    public async Task DeleteVacationRequestReviewAsync(int id)
    {
        VacationRequestReview? review = await _context.VacationRequestsReviews.FindAsync(id);

        if (review is null)
        {
            return;
        }

        _context.VacationRequestsReviews.Remove(review);
    }
}
