using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
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
        Positions = await _unitOfWork.PositionService.GetAllAsync();
        ContractTypes = await _unitOfWork.ContractService.GetContractTypesAsync();

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

        ContractDTO.StartDate = EmployeeDTO.EmploymentStartDate;

        ContractDTO.EmployeeId = employeeFromDb.Id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
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

    public async Task<IActionResult> OnPostAddContractAsync()
    {
        var employee = await _unitOfWork.EmployeeService.GetByIdAsync(ContractDTO.EmployeeId);
        if (employee is null)
        {
            return Unauthorized();
        }

        ContractDTO.EmployeeId = employee.Id;

        Positions = await _unitOfWork.PositionService.GetAllAsync();
        ContractTypes = await _unitOfWork.ContractService.GetContractTypesAsync();

        Uri contractFileUrl = await _unitOfWork.FileService.SaveFile(ContractFile);

        var contract = new Contract()
        {
            Employee = employee,
            Number = ContractDTO.Number,
            StartDate = ContractDTO.StartDate,
            EndDate = ContractDTO.EndDate,
            Type = ContractTypes.Single(ct => ct.Id == ContractDTO.ContractTypeId),
            DocumentUrl = contractFileUrl.ToString()
        };

        var contractValidationResult = _unitOfWork.ContractService.CreateContract(contract);

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

        return RedirectToAction("Index", nameof(Contracts));
    }
}
