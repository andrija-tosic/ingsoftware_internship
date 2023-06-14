﻿using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.Email).EmailAddress();
        RuleFor(e => e.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(e => e.LastName).NotEmpty().MinimumLength(3);
        RuleFor(e => e.Address).NotEmpty().MinimumLength(3);
        RuleFor(e => e.IdNumber).NotEmpty();
        RuleFor(e => e.DaysOffNumber).GreaterThanOrEqualTo(0);
        When(e => e.EmploymentEndDate is not null, () =>
        {
            RuleFor(e => e.EmploymentEndDate).GreaterThan(e => e.EmploymentStartDate)
            .WithMessage("Employment end date must be after the start date.");
        });
    }
}
