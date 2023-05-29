using Microsoft.AspNetCore.Identity;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUserStore<Employee> _userStore;
    private readonly UserManager<Employee> _userManager;
    private readonly IUserEmailStore<Employee> _emailStore;

    public EmployeeService(IUserStore<Employee> userStore, UserManager<Employee> userManager)
    {
        _userStore = userStore;
        _userManager = userManager;
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        _emailStore = (IUserEmailStore<Employee>)_userStore;
    }

    public async Task<Employee?> Get(string id)
    {
        return await _userStore.FindByIdAsync(id, CancellationToken.None);
    }

    public async Task<IdentityResult> Create(Employee employee, string password)
    {
        if (employee.Position.Caption == Positions.HR)
        {
            await _userManager.AddToRoleAsync(employee, nameof(UserRoles.Administrator));
        }

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);
        IdentityResult result = await _userManager.CreateAsync(employee, password);

        return result;
    }

    public async Task<IdentityResult> Update(Employee employee)
    {
        if (employee.Position.Caption == Positions.HR)
        {
            await _userManager.AddToRoleAsync(employee, nameof(UserRoles.Administrator));
        }

        IdentityResult result = await _userManager.UpdateAsync(employee);

        return result;
    }

    public async Task<IdentityResult> Delete(Employee employee)
    {
        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);
        IdentityResult result = await _userManager.DeleteAsync(employee);

        return result;
    }
}
