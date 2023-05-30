using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Data;
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

        public IList<Employee> Employees { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Employees = await _unitOfWork.EmployeeService.GetAll();
        }
    }
}
