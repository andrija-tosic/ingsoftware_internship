using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages;

[Authorize(Roles = InitialData.AdminRoleName)]
public class EditModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public EditModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public EmployeeDTO EmployeeDTO { get; set; } = default!;
    public IEnumerable<Position> Positions { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        Positions = await _unitOfWork.PositionService.GetAllAsync();

        if (id is null)
        {
            return NotFound();
        }

        var employeeFromDb = await _unitOfWork.EmployeeService.GetByIdAsync(id);
        if (employeeFromDb is null)
        {
            return NotFound();
        }
        EmployeeDTO = employeeFromDb;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var employeeFromDb = await _unitOfWork.EmployeeService.GetByIdAsync(EmployeeDTO.Id);

        if (employeeFromDb is null)
        {
            return (IActionResult)IdentityResult.Failed(new IdentityError());
        }

        employeeFromDb.Address = EmployeeDTO.Address;
        employeeFromDb.Id = EmployeeDTO.Id;
        employeeFromDb.DaysOffNumber = EmployeeDTO.DaysOffNumber;
        employeeFromDb.EmploymentStartDate = EmployeeDTO.EmploymentStartDate;
        employeeFromDb.EmploymentEndDate = EmployeeDTO.EmploymentEndDate;
        employeeFromDb.FirstName = EmployeeDTO.FirstName;
        employeeFromDb.LastName = EmployeeDTO.LastName;
        employeeFromDb.Email = EmployeeDTO.Email;
        employeeFromDb.IdNumber = EmployeeDTO.IdNumber;
        employeeFromDb.Position = (await _unitOfWork.PositionService.GetByIdAsync(EmployeeDTO.PositionId))!;

        var validationResult = await _unitOfWork.EmployeeService.UpdateAsync(employeeFromDb);

        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            Positions = await _unitOfWork.PositionService.GetAllAsync();

            return Page();
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
