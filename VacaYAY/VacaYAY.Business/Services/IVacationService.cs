using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IVacationService
{
    Task<VacationRequest?> GetVacationRequestByIdAsync(int id);
    void CreateVacationRequest(VacationRequest vacationRequest);
    void UpdateVacationRequest(VacationRequest vacationRequest);
    Task DeleteVacationRequestAsync(int id);
    Task<IList<VacationRequest>> GetAllVacationRequestsAsync(string employeeId, bool isAdmin);
    Task<IList<LeaveType>> GetLeaveTypes();
    Task<LeaveType?> GetLeaveTypeById(int id);
    void CreateVacationRequestReview(VacationRequestReview vacationRequestReview);
    void UpdateVacationRequestReview(VacationRequestReview vacationRequestReview);
    Task DeleteVacationRequestReviewAsync(int id);
}
