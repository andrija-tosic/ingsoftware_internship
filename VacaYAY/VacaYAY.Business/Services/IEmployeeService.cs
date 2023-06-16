using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IEmployeeService
{
    Task<Employee?> GetByIdAsync(string id);
    Task<Employee?> GetLoggedInAsync(ClaimsPrincipal claims);
    Task<ValidationResult> CreateAsync(Employee employee, string password);
    Task<ValidationResult> UpdateAsync(Employee employee);
    Task<IdentityResult> SoftDeleteAsync(Employee employee);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Employee>> SearchAsync(EmployeeSearchFilters searchFilters);
    Task<ValidationResult> CreateFakesAsync(int count);
    Task<bool> IsInRoleAsync(Employee employee, string role);
    Task<IEnumerable<Employee>> GetByPositions(int[] positions);
}
