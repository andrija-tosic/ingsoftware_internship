using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IEmployeeService
{
    Task<Employee?> GetByIdAsync(string id);
    Task<Employee?> GetLoggedInAsync(ClaimsPrincipal claims);
    Task<IdentityResult> CreateAsync(Employee employee, string password);
    Task<IdentityResult> UpdateAsync(Employee employee);
    Task<IdentityResult> SoftDeleteAsync(Employee employee);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Employee>> SearchAsync(string firstName, string lastName, DateTime? employmentStart, DateTime? employmentEnd);
    Task<IdentityResult> CreateFakesAsync(int count);
    IEnumerable<Employee> GenerateFakes(int count, IList<Position> positions);
    Task<bool> IsInRoleAsync(Employee employee, string role);
}
