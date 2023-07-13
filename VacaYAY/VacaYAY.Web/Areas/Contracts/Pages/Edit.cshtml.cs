using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Contracts.Pages;


[Authorize(Roles = InitialData.AdminRoleName)]
public class EditModel : PageModel
{
    private readonly IContractService _contractService;
    private readonly IEmployeeService _employeeService;
    private readonly IFileService _fileService;
    private readonly IHttpService _httpService;

    public EditModel(
        IContractService contractService,
        IEmployeeService employeeService,
        IHttpService httpService)
    {
        _contractService = contractService;
        _employeeService = employeeService;
        _fileService = new FileService("UseDevelopmentStorage=true");
        _httpService = httpService;
    }

    [BindProperty(SupportsGet = true)]
    public ContractDTO ContractDTO { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public Contract Contract { get; set; } = default!;
    public IList<ContractType> ContractTypes { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        ContractTypes = await _contractService.GetContractTypesAsync();

        if (id is null)
        {
            return NotFound();
        }

        var contract = await _contractService.GetByIdAsync((int)id);
        if (contract is null)
        {
            return NotFound();
        }

        Contract = contract;

        ContractDTO.ContractFile = await _httpService.GetFormFileAsync(contract.DocumentUrl);

        ContractDTO = new ContractDTO()
        {
            Id = contract.Id,
            ContractTypeId = contract.Type.Id,
            EmployeeId = contract.Employee.Id,
            Number = contract.Number,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var contractValidationResult = await _contractService.UpdateContractAsync(ContractDTO);

        if (!contractValidationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in contractValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Page();
        }

        Contract = await _contractService.GetByIdAsync(Contract.Id);
        ContractTypes = await _contractService.GetContractTypesAsync();

        return RedirectToPage("./Index");
    }
}
