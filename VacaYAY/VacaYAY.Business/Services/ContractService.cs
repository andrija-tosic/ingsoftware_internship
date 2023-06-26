using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacaYAY.Business.Validators;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class ContractService : IContractService
{
    private readonly DbSet<Contract> _contracts;
    private readonly DbSet<ContractType> _contractTypes;
    private readonly ILogger<IContractService> _logger;

    public ContractService(DbSet<Contract> contracts, DbSet<ContractType> contractTypes, ILogger<IContractService> logger)
    {
        _contracts = contracts;
        _contractTypes = contractTypes;
        _logger = logger;
    }
    public async Task<IList<Contract>> GetAllAsync()
    {
        return await _contracts
            .Include(c => c.Employee)
            .ToListAsync();
    }

    public async Task<IList<ContractType>> GetContractTypesAsync()
    {
        return await _contractTypes.ToListAsync();
    }

    public ValidationResult CreateContract(Contract contract)
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

        _contracts.Add(contract);
        return validationResult;
    }
    public ValidationResult UpdateContract(Contract contract)
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
        
        _contracts.Update(contract);

        return validationResult;
    }

    public async Task DeleteContract(int id)
    {
        var contract = await _contracts
            .Where(v => v.Id == id)
            .SingleAsync();

        if (contract is null)
        {
            return;
        }

        _contracts.Remove(contract);
    }

    public async Task<Contract> GetByIdAsync(int id)
    {
        return await _contracts
            .Include(c => c.Employee)
            .Include(c => c.Type)
            .SingleAsync(c => c.Id == id);
    }
}
