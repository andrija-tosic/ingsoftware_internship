using FluentValidation.Results;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IVacationService
{
    Task<VacationRequest?> GetVacationRequestByIdAsync(int id);
    Task<ValidationResult> CreateVacationRequestAsync(VacationRequest vacationRequest);
    void UpdateVacationRequest(VacationRequest vacationRequest);
    Task DeleteVacationRequestAsync(int id);
    Task<IList<VacationRequest>> SearchVacationRequestsAsync(string employeeId, bool isAdmin, VacationRequestSearchFilters searchFilters);
    Task<IList<LeaveType>> GetLeaveTypesAsync();
    Task<LeaveType?> GetLeaveTypeByIdAsync(int id);
    void CreateVacationReview(VacationReview vacationRequestReview);
    void UpdateVacationReview(VacationReview vacationRequestReview);
    Task DeleteVacationReviewAsync(int id);
    Task<VacationReview?> GetVacationReviewByIdAsync(int id);
    PdfDocument GenerateVacationReportPdf(VacationReview vacationReview);
}
