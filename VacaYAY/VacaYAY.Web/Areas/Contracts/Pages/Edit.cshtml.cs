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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpService _httpService;

    public EditModel(IUnitOfWork unitOfWork, IHttpService httpService)
    {
        _unitOfWork = unitOfWork;
        _httpService = httpService;
    }

    [BindProperty(SupportsGet = true)]
    public ContractDTO ContractDTO { get; set; } = default!;
    [BindProperty]
    public IFormFile? ContractFile { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public Contract Contract { get; set; } = default!;
    public IList<ContractType> ContractTypes { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        ContractTypes = await _unitOfWork.ContractService.GetContractTypesAsync();

        if (id is null)
        {
            return NotFound();
        }

        var contract = await _unitOfWork.ContractService.GetByIdAsync((int)id);
        if (contract is null)
        {
            return NotFound();
        }

        Contract = contract;

        ContractFile = await _httpService.GetFormFileAsync(contract.DocumentUrl);

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
        var contract = await _unitOfWork.ContractService.GetByIdAsync(ContractDTO.Id);
        if (contract is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.EmployeeService.GetByIdAsync(ContractDTO.EmployeeId);
        if (employee is null)
        {
            return Unauthorized();
        }

        ContractTypes = await _unitOfWork.ContractService.GetContractTypesAsync();

        string contractFileUrl = contract.DocumentUrl;

        if (ContractFile is not null)
        {
            // Contract file changed.
            Uri uri = await _unitOfWork.FileService.SaveFile(ContractFile);

            contractFileUrl = uri.ToString();
        }

        contract.Employee = employee;
        contract.Number = ContractDTO.Number;
        contract.StartDate = ContractDTO.StartDate;
        contract.EndDate = ContractDTO.EndDate;
        contract.Type = ContractTypes.Single(ct => ct.Id == ContractDTO.ContractTypeId);
        contract.DocumentUrl = contractFileUrl;

        Contract = contract;

        var contractValidationResult = _unitOfWork.ContractService.UpdateContract(contract);

        if (!contractValidationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in contractValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Page();
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
