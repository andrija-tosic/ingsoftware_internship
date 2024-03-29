﻿using FluentValidation;
using VacaYAY.Business.Services;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Validators;

public class VacationRequestValidator : AbstractValidator<VacationRequest>
{
    private readonly IVacationService _vacationService;

    public VacationRequestValidator(IVacationService vacationService)
    {
        _vacationService = vacationService;

        RuleFor(v => v.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Date.AddDays(1))
            .GreaterThan(v => v.Employee.EmploymentStartDate.Date)
            .WithMessage("Vacation start date must be after employment start date.")
            .LessThan(v => v.Employee.EmploymentEndDate!.Value.Date)
            .WithMessage("Vacation end date must be before employment end date.");

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .GreaterThan(v => v.StartDate)
            .WithMessage("Vacation end date must be after the start date.");

        RuleFor(v => v).MustAsync(async (newRequest, cancellation) =>
        {
            int newRequestDays = (newRequest.EndDate.Date - newRequest.StartDate.Date).Days;

            var employeesVacationRequests = await _vacationService.SearchVacationRequestsAsync(
                newRequest.Employee.Id, false, new VacationRequestSearchFilters { });

            var otherRequestsWithPendingReview = employeesVacationRequests
                .Where(r => r.VacationReview is null && r.Id != newRequest.Id);

            VacationRequest? existingRequest = employeesVacationRequests
                .SingleOrDefault(r => r.VacationReview is null && r.Id == newRequest.Id);

            int usedDays = newRequestDays + otherRequestsWithPendingReview.Sum(r => (r.EndDate.Date - r.StartDate.Date).Days);

            int totalDaysOffNumber = newRequest.Employee.DaysOffNumber + newRequest.Employee.LastYearsDaysOffNumber;

            return totalDaysOffNumber - usedDays >= 0;
        }).WithMessage(v => $"Employee {v.Employee.FirstName} {v.Employee.LastName} has only {v.Employee.DaysOffNumber} days off left.");

        RuleFor(v => v.Employee).NotNull();

        RuleFor(v => v).MustAsync(async (newRequest, cancellation) =>
        {
            IList<VacationRequest> otherRequests = (await _vacationService.SearchVacationRequestsAsync(
                newRequest.Employee.Id, false, new VacationRequestSearchFilters()))
                .Where(req => (newRequest.Id != req.Id)
                           && (req.VacationReview == null || req.VacationReview.Approved))
                .ToList();

            return otherRequests.All(req =>
            {
                return
                (newRequest.StartDate < req.StartDate && newRequest.EndDate < req.StartDate)
             || (newRequest.StartDate > req.EndDate && newRequest.EndDate > req.EndDate);

            });
        }).WithMessage("Vacation dates overlap with an existing vacation request.");
    }
}
