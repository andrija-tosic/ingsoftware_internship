using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages
{
    [Authorize(Roles = InitialData.AdminRoleName)]
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
      public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var employee = await _unitOfWork.EmployeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            else 
            {
                Employee = employee;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var employee = await _unitOfWork.EmployeeService.GetByIdAsync(id);

            if (employee != null)
            {
                Employee = employee;
                
                await _unitOfWork.EmployeeService.SoftDeleteAsync(employee);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
