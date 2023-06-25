using FluentValidation.Results;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IContractService
{
    Task<IList<ContractType>> GetContractTypesAsync();
    Task<IList<Contract>> GetAllAsync();
    ValidationResult CreateContract(Contract contract);
    ValidationResult UpdateContract(Contract contract);
    Task DeleteContract(int id);
    Task<Contract> GetByIdAsync(int id);
}
