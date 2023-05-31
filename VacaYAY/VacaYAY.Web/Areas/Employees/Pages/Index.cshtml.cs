using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public class SearchInput
        {
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public DateTime? EmploymentStartDate { get; set; }
            public DateTime? EmploymentEndDate { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public SearchInput Input { get; set; } = default!;

        public IList<Employee> Employees { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Employees = await _unitOfWork.EmployeeService.SearchAsync(Input.FirstName, Input.LastName, Input.EmploymentStartDate, Input.EmploymentEndDate);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Employees = await _unitOfWork.EmployeeService.SearchAsync(Input.FirstName, Input.LastName, Input.EmploymentStartDate, Input.EmploymentEndDate);

            return Page();
        }
    }

}
