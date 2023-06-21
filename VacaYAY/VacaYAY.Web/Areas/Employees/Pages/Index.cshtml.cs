using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages;

[Authorize(Roles = InitialData.AdminRoleName)]
public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty(SupportsGet = true)]
    public EmployeeSearchFilters Input { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int NumberOfFakeEmployeesToGenerate { get; set; }

    public IList<Employee> Employees { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Employees = await _unitOfWork.EmployeeService.SearchAsync(Input);
    }

    public async Task<IActionResult> OnPostSearchAsync()
    {
        Employees = await _unitOfWork.EmployeeService.SearchAsync(Input);

        return Page();
    }

    public async Task<IActionResult> OnPostGenerateFakeEmployeesAsync()
    {
        await _unitOfWork.EmployeeService.CreateFakesAsync(NumberOfFakeEmployeesToGenerate);
        await _unitOfWork.SaveChangesAsync();

        Employees = await _unitOfWork.EmployeeService.SearchAsync(Input);

        return Page();
    }
}
