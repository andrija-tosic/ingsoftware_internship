using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IEmployeeService
{
    public Task<Employee?> Get(string id);
    public Task<IdentityResult> Create(Employee employee, string password);
    public Task<IdentityResult> Update(Employee employee);
    public Task<IdentityResult> Delete(Employee employee);
}