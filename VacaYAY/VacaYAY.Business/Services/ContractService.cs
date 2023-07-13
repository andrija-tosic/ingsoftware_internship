using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacaYAY.Business.Validators;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class ContractService : IContractService
{
    private readonly VacayayDbContext _context;
    private readonly IFileService _fileService;
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<IContractService> _logger;

    public ContractService(
        VacayayDbContext context,
        IUserStore<Employee> userStore,
        UserManager<Employee> userManager,
        IHttpService httpService,
        IFileService fileService,
        ILogger<IContractService> logger
        )
    {
        _context = context;
        _fileService = fileService;
        _employeeService = new EmployeeService(context, userStore, userManager, httpService);
        _logger = logger;
    }
    public async Task<IList<Contract>> GetAllAsync()
    {
        return await _context.Contracts
            .Include(c => c.Employee)
            .ToListAsync();
    }

    public async Task<IList<ContractType>> GetContractTypesAsync()
    {
        return await _context.ContractTypes.ToListAsync();
    }

    public async Task<ValidationResult> CreateContractAsync(Contract contract)
    {
        var validationResult = new ContractValidator().Validate(contract);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return validationResult;
        }

        _context.Add(contract);
        await _context.SaveChangesAsync();

        return validationResult;
    }
    public async Task<ValidationResult> UpdateContractAsync(ContractDTO contractDto)
    {
        var contract = await GetByIdAsync(contractDto.Id);
        // TODO
        //if (contract is null)
        //{
        //    return NotFound();
        //}

        string contractFileUrl = contract.DocumentUrl;

        if (contractDto.ContractFile is not null)
        {
            // Contract file changed.
            Uri uri = await _fileService.SaveFileAsync(contractDto.ContractFile);

            contractFileUrl = uri.ToString();
        }

        var employee = await _employeeService.GetByIdAsync(contractDto.EmployeeId);
        // TODO
        //if (employee is null)
        //{
        //    return Unauthorized();
        //}
        contract.Employee = employee;
        contract.Number = contractDto.Number;
        contract.StartDate = contractDto.StartDate;
        contract.EndDate = contractDto.EndDate;
        contract.Type = _context.ContractTypes.Single(ct => ct.Id == contractDto.ContractTypeId);
        contract.DocumentUrl = contractFileUrl;

        var validationResult = new ContractValidator().Validate(contract);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return validationResult;
        }

        _context.Update(contract);
        await _context.SaveChangesAsync();

        return validationResult;
    }

    public async Task DeleteContractAsync(int id)
    {
        var contract = await _context.Contracts
            .Where(v => v.Id == id)
            .SingleAsync();

        if (contract is null)
        {
            return;
        }

        _context.Remove(contract);
        await _context.SaveChangesAsync();
    }

    public async Task<Contract> GetByIdAsync(int id)
    {
        return await _context.Contracts
            .Include(c => c.Employee)
            .Include(c => c.Type)
            .SingleAsync(c => c.Id == id);
    }
}
