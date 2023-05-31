using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages
{
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public EmployeeDTO EmployeeDTO { get; set; } = default!;
        public List<Position> Positions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Positions = await _unitOfWork.PositionService.GetAllAsync();

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _unitOfWork.EmployeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            EmployeeDTO = employee;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
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
            employeeFromDb.InsertDate = EmployeeDTO.InsertDate;
            employeeFromDb.Position = (await _unitOfWork.PositionService.GetByIdAsync(EmployeeDTO.PositionId))!;

            try
            {
                IdentityResult res = await _unitOfWork.EmployeeService.UpdateAsync(employeeFromDb);
                if (res.Succeeded)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
