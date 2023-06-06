using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class VacationRequestValidator : AbstractValidator<VacationRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public VacationRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(v => v.EndDate)
            .NotEmpty()
            .GreaterThan(v => v.StartDate)
            .NotEmpty();

        RuleFor(v => v.Comment).NotEmpty();
        RuleFor(v => v.Employee).NotNull();

        RuleFor(v => v).MustAsync(async (newRequest, cancellation) =>
        {
            IList<VacationRequest> otherRequests = await _unitOfWork.VacationService.GetAllVacationRequestsAsync(newRequest.Employee.Id, false);

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
        });
    }
}
