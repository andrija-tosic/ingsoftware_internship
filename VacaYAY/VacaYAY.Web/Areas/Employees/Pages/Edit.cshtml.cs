using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Business;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Employees.Pages;

[Authorize(Roles = InitialData.AdminRoleName)]
public class EditModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IContractService _contractService;
    private readonly IFileService _fileService;

    public EditModel(
        IEmployeeService employeeService,
        IPositionService positionService,
        IContractService contractService,
        IFileService fileService
        )
    {
        _employeeService = employeeService;
        _positionService = positionService;
        _contractService = contractService;
        _fileService = fileService;
    }

    [BindProperty(SupportsGet = true)]
    public EmployeeDTO EmployeeDTO { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public ContractDTO ContractDTO { get; set; } = default!;
    [BindProperty]
    [Required(ErrorMessage = "Contract document is required.")]
    public required IFormFile ContractFile { get; set; }
    public IList<ContractType> ContractTypes { get; set; } = default!;
    public IList<Position> Positions { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        Positions = await _positionService.GetAllAsync();
        ContractTypes = await _contractService.GetContractTypesAsync();

        if (id is null)
        {
            return NotFound();
        }

        var employeeFromDb = await _employeeService.GetByIdAsync(id);
        if (employeeFromDb is null)
        {
            return NotFound();
        }
        EmployeeDTO = employeeFromDb;

        ContractDTO.StartDate = EmployeeDTO.EmploymentStartDate;

        ContractDTO.EmployeeId = employeeFromDb.Id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Positions = await _positionService.GetAllAsync();

        var validationResult = await _employeeService.UpdateAsync(EmployeeDTO);

        if (!validationResult.IsValid)
        {
            ModelState.Clear();

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }


            return Page();
        }

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostAddContractAsync()
    {
        ContractDTO.ContractFile = ContractFile;
        var contractValidationResult = await _contractService.CreateContractAsync(ContractDTO);

        if (!contractValidationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in contractValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return Page();
        }

        return RedirectToAction("Index", nameof(Contracts));
    }
}
