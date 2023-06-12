using FluentValidation;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class VacationRequestValidator : AbstractValidator<VacationRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public VacationRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(v => v.StartDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Now.Date.AddDays(1));

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .GreaterThan(v => v.StartDate)
            .NotEmpty()
            .WithMessage("Vacation end date must be after the start date.");

        RuleFor(v => v).MustAsync(async (v, cancellation) =>
        {
            int newRequestDays = (v.EndDate - v.StartDate).Days;

            IList<VacationRequest> employeesVacationRequests = await _unitOfWork.VacationService.SearchVacationRequestsAsync(v.Employee.Id, false, new VacationRequestSearchFilters { });

            var requestsWithPendingReview = employeesVacationRequests.Where(r => r.VacationReview is null);
            int usedDays = newRequestDays + requestsWithPendingReview.Sum(r => (r.EndDate - r.StartDate).Days);

            return v.Employee.DaysOffNumber - usedDays > 0;
        }).WithMessage((v) => $"Employee {v.Employee.FirstName} {v.Employee.LastName} has only {v.Employee.DaysOffNumber} days off left.");

        RuleFor(v => v.Comment).NotEmpty();
        RuleFor(v => v.Employee).NotNull();

        RuleFor(v => v).MustAsync(async (newRequest, cancellation) =>
        {
            IList<VacationRequest> otherRequests = await _unitOfWork.VacationService.SearchVacationRequestsAsync(newRequest.Employee.Id, false,
                new VacationRequestSearchFilters());

            return otherRequests.All(req =>
            {
                if (newRequest.Id != req.Id)
                {
                    return
                    (newRequest.StartDate < req.StartDate && newRequest.EndDate < req.StartDate)
                 || (newRequest.StartDate > req.EndDate && newRequest.EndDate > req.EndDate);
                }
                else
                {
                    return true;
                }
            });
        }).WithMessage("Vacation dates overlap with an existing vacation request.");
    }
}
