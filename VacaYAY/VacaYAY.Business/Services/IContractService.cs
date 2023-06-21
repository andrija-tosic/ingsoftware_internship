using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IContractService
{
    Task<IList<ContractType>> GetContractTypesAsync();
    Task<IList<Contract>> GetContractsAsync();
}
