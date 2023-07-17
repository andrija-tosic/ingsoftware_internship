using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages;

[Authorize(Roles = InitialData.AdminRoleName)]
public class IndexModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;

    public IndexModel(
        IEmployeeService employeeService,
        IPositionService positionService)
    {
        _employeeService = employeeService;
        _positionService = positionService;
    }

    [BindProperty(SupportsGet = true)]
    public EmployeeSearchFilters Input { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int NumberOfFakeEmployeesToGenerate { get; set; }

    public IList<Employee> Employees { get; set; } = default!;
    public IList<Position> Positions { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Employees = await _employeeService.SearchAsync(Input);
        Positions = await _positionService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostSearchAsync()
    {
        Employees = await _employeeService.SearchAsync(Input);
        Positions = await _positionService.GetAllAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostGenerateFakeEmployeesAsync()
    {
        await _employeeService.CreateFakesAsync(NumberOfFakeEmployeesToGenerate);

        Employees = await _employeeService.SearchAsync(Input);
        Positions = await _positionService.GetAllAsync();

        return Page();
    }
}
