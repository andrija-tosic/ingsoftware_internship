using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IEmployeeService
{
    public Task<Employee?> GetByIdAsync(string id);
    public Task<IdentityResult> CreateAsync(Employee employee, string password);
    public Task<IdentityResult> UpdateAsync(Employee employee);
    public Task<IdentityResult> SoftDeleteAsync(Employee employee);
    Task<IList<Employee>> GetAll();
}