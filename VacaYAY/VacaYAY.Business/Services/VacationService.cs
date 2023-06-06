using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class VacationService : IVacationService
{
    private readonly VacayayDbContext _context;
    private readonly IValidator<VacationRequest> _vacationRequestValidator;

    public VacationService(VacayayDbContext context, IValidator<VacationRequest> vacationRequestValidator)
    {
        _context = context;
        _vacationRequestValidator = vacationRequestValidator;
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

    public void CreateVacationRequest(VacationRequest vacationRequest)
    {
        var validationResult = _vacationRequestValidator.Validate(vacationRequest);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.ToString());
        }

        _context.VacationRequests.Add(vacationRequest);
    }
    public void UpdateVacationRequest(VacationRequest vacationRequest)
    {
        var validationResult = _vacationRequestValidator.Validate(vacationRequest);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.ToString());
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
}
