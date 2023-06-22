using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.Email).EmailAddress();
        RuleFor(e => e.FirstName)
            .MinimumLength(3).WithMessage("First name must be at least 3 letters long.")
            .Must(name => name.All(c => char.IsLetter(c)))
            .WithMessage("First name must contain letters only.");

        RuleFor(e => e.LastName)
            .MinimumLength(3).WithMessage("Last name must be at least 3 letters long.")
            .Must(name => name.All(c => char.IsLetter(c)))
            .WithMessage("Last name must contain letters only."); ;

        RuleFor(e => e.Address).NotEmpty().MinimumLength(3);

        RuleFor(e => e.IdNumber)
            .NotEmpty()
            .Matches(@"^[0-9]+$")
            .WithMessage("ID number must contain numbers only.");

        RuleFor(e => e.DaysOffNumber).GreaterThanOrEqualTo(0);

        When(e => e.EmploymentEndDate is not null, () =>
        {
            RuleFor(e => e.EmploymentEndDate).GreaterThan(e => e.EmploymentStartDate)
            .WithMessage("Employment end date must be after the start date.");
        });
    }
}
