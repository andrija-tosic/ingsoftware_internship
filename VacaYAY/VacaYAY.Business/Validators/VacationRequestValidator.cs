using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class VacationRequestValidator : AbstractValidator<VacationRequest>
{
    public VacationRequestValidator()
    {
        RuleFor(v => v.EndDate)
            .NotEmpty()
            .GreaterThan(v => v.StartDate)
            .NotEmpty();
    
        RuleFor(v => v.Comment).NotEmpty();
        RuleFor(v => v.Employee).NotNull();
    }
}
