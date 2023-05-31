using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(e => e.LastName).NotEmpty().MinimumLength(3);
        RuleFor(e => e.Address).NotEmpty().MinimumLength(3);
        RuleFor(e => e.IdNumber).NotEmpty();
        RuleFor(e => e.DaysOffNumber).GreaterThanOrEqualTo(0);
        When(e => e.EmploymentEndDate is not null, () =>
        {
            RuleFor(e => e.EmploymentStartDate).GreaterThan(e => e.EmploymentEndDate);
        });
    }
}
