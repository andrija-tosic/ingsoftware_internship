using FluentValidation;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class ContractValidator : AbstractValidator<Contract>
{
    public ContractValidator()
    {
        RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate)
            .WithMessage("Contract end date must be after the start date.");

        RuleFor(c => c.Employee).NotNull();
        RuleFor(c => c.DocumentUri).Must(uri =>
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
