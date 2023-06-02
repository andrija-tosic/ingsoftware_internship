using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IEmployeeService
{
    Task<Employee?> GetByIdAsync(string id);
    Task<IdentityResult> CreateAsync(Employee employee, string password);
    Task<IdentityResult> UpdateAsync(Employee employee);
    Task<IdentityResult> SoftDeleteAsync(Employee employee);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Employee>> SearchAsync(string firstName, string lastName, DateTime? employmentStart, DateTime? employmentEnd);
    IList<Employee> GetRandoms(int count);
    Task<IdentityResult> CreateFakes(int count);
}