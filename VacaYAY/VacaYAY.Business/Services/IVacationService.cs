using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IVacationService
{
    Task<VacationRequest?> GetVacationRequestByIdAsync(int id);
    Task CreateVacationRequest(VacationRequest vacationRequest);
    Task UpdateVacationRequest(VacationRequest vacationRequest);
    Task DeleteVacationRequestAsync(int id);
    Task<IList<VacationRequest>> SearchVacationRequestsAsync(string employeeId, bool isAdmin, VacationRequestSearchFilters searchFilters);
    Task<IList<LeaveType>> GetLeaveTypes();
    Task<LeaveType?> GetLeaveTypeById(int id);
    void CreateVacationRequestReview(VacationRequestReview vacationRequestReview);
    void UpdateVacationRequestReview(VacationRequestReview vacationRequestReview);
    Task DeleteVacationRequestReviewAsync(int id);
}
