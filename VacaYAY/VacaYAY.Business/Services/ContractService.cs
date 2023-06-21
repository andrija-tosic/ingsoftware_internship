using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class ContractService : IContractService
{
    private readonly DbSet<Contract> _contracts;
    private readonly DbSet<ContractType> _contractTypes;

    public ContractService(DbSet<Contract> contracts, DbSet<ContractType> contractTypes)
    {
        _contracts = contracts;
        _contractTypes = contractTypes;
    }
    public async Task<IList<Contract>> GetContractsAsync()
    {
        return await _contracts
            .Include(c => c.Employee)
            .ToListAsync();
    }

    public async Task<IList<ContractType>> GetContractTypesAsync()
    {
        return await _contractTypes.ToListAsync();
    }
}
