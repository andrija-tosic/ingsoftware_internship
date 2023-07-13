using FluentValidation.Results;
using System.Security.Claims;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IVacationService
{
    Task<VacationRequest?> GetVacationRequestByIdAsync(int id);
    Task<ValidationResult> CreateVacationRequestAsync(VacationRequest vacationRequest, ClaimsPrincipal AuthenticatedUser);
    Task<ValidationResult> UpdateVacationRequestAsync(VacationRequestDTO vacationRequest, ClaimsPrincipal AuthenticatedUser);
    Task DeleteVacationRequestAsync(int id, ClaimsPrincipal AuthenticatedUser);
    Task<IList<VacationRequest>> SearchVacationRequestsAsync(string employeeId, bool isAdmin, VacationRequestSearchFilters searchFilters);
    Task<IList<LeaveType>> GetLeaveTypesAsync();
    Task<LeaveType?> GetLeaveTypeByIdAsync(int id);
    Task CreateVacationReviewAsync(VacationReview vacationReview, int vacationRequestId, ClaimsPrincipal AuthenticatedUser);
    Task UpdateVacationReviewAsync(VacationReview vacationReview, int vacationRequestId, ClaimsPrincipal AuthenticatedUser);
    Task DeleteVacationReviewAsync(int id, ClaimsPrincipal AuthenticatedUser);
    Task<VacationReview?> GetVacationReviewByIdAsync(int id);
    Task<int> GetPotentiallyUsedDaysAsync(string employeeId);
    PdfDocument GenerateVacationReportPdf(VacationReview vacationReview);
    void SendVacationReportEmail(VacationReview vacationReview, PdfDocument vacationReportPdf);
}
