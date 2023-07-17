using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IContractService
{
    Task<IList<ContractType>> GetContractTypesAsync();
    Task<IList<Contract>> GetAllAsync();
    Task<ValidationResult> CreateContractAsync(ContractDTO contractDto);
    Task<ValidationResult> UpdateContractAsync(ContractDTO contractDto);
    Task DeleteContractAsync(int id);
    Task<Contract> GetByIdAsync(int id);
}
