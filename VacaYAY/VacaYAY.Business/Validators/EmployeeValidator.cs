using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Address).NotEmpty().MinimumLength(3);
        RuleFor(x => x.IdNumber).NotEmpty();
        RuleFor(x => x.DaysOffNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EmploymentStartDate).GreaterThan(x => x.EmploymentEndDate);
    }
}
