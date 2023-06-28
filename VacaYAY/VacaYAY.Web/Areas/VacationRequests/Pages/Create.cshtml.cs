﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class CreateModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public VacationRequest VacationRequest { get; set; } = default!;

    public IList<LeaveType> LeaveTypes { get; set; } = default!;

    [BindProperty]
    public int LeaveTypeId { get; set; } = default;
    public int PotentialDaysOff { get; set; } = default;

    public async Task<IActionResult> OnGetAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypesAsync();


        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return Unauthorized();
        }

        int potentiallyUsedDays = await _unitOfWork.VacationService.GetPotentiallyUsedDaysAsync(loggedInEmployee.Id);

        PotentialDaysOff = loggedInEmployee.DaysOffNumber - potentiallyUsedDays;

        VacationRequest = new()
        {
            Comment = string.Empty,
            Employee = loggedInEmployee,
            StartDate = DateTime.Now.Date.AddDays(1),
            EndDate = DateTime.Now.Date.AddDays(6),
            LeaveType = LeaveTypes.FirstOrDefault()!
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypesAsync();

        LeaveType? leaveType = await _unitOfWork.VacationService.GetLeaveTypeByIdAsync(LeaveTypeId);

        if (leaveType is null)
        {
            return NotFound();
        }

        VacationRequest.LeaveType = leaveType;

        Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

        if (loggedInEmployee is null)
        {
            return NotFound();
        }

        int potentiallyUsedDays = await _unitOfWork.VacationService.GetPotentiallyUsedDaysAsync(loggedInEmployee.Id);

        PotentialDaysOff = loggedInEmployee.DaysOffNumber - potentiallyUsedDays;

        VacationRequest.Employee = loggedInEmployee;
        var requestValidationResult = await _unitOfWork.VacationService.CreateVacationRequestAsync(VacationRequest);

        ModelState.Clear();
        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return Page();
        }

        int days = (VacationRequest.EndDate - VacationRequest.StartDate).Days;

        await _unitOfWork.SaveChangesAsync();

        var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

        string emailSubject = $"Vacation requested by {VacationRequest.Employee.FirstName} {VacationRequest.Employee.LastName}";
        string emailBody = VacationRequest.ToString();
        emailBody += $@"

<a href=""https://localhost:7085/{nameof(VacationRequests)}/Details?id={VacationRequest.Id}"">
Go to details page
</a>
";

        foreach (var e in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
        }

        _unitOfWork.EmailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

        return RedirectToPage("./Index");
    }
}
