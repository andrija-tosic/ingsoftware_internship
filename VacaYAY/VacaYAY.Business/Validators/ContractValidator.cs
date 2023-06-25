using FluentValidation;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class ContractValidator : AbstractValidator<Contract>
{
    public ContractValidator()
    {
        RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate)
            .WithMessage("Contract end date must be after the start date.");

        RuleFor(c => c).Must(c =>
        {
            if (!c.EndDate.HasValue)
            {
                return c.Type.Id == InitialData.OpenEndedContractType.Id;
            }
            return true;
        }).WithMessage($"Contract must have end date if the contract type is not {InitialData.OpenEndedContractType.Name}.");

        RuleFor(c => c.StartDate)
            .GreaterThanOrEqualTo(c => c.Employee.EmploymentStartDate)
            .WithMessage("Contract start date must be after employee's employment starts.");

        RuleFor(c => c.EndDate)
            .LessThanOrEqualTo(c => c.Employee.EmploymentEndDate)
            .WithMessage("Contract end date must be before employee's employment ends.");

        RuleFor(c => c.Employee).NotNull();
        RuleFor(c => c.DocumentUrl).Must(uri =>
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                return false;
            }

            return Uri.TryCreate(uri, UriKind.Absolute, out Uri? outUri)
                && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }).WithMessage("Document URI is not a valid URI.");

        RuleFor(c => c.Number).Length(6);
    }
}
