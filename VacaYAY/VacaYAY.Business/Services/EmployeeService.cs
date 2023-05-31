﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class EmployeeService : IEmployeeService
{
    private readonly VacayayDbContext _context;
    private readonly IUserStore<Employee> _userStore;
    private readonly UserManager<Employee> _userManager;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IUserEmailStore<Employee> _emailStore;

    public EmployeeService(VacayayDbContext context, IUserStore<Employee> userStore, UserManager<Employee> userManager,
        IValidator<Employee> employeeValidator)
    {
        _context = context;
        _userStore = userStore;
        _userManager = userManager;
        _employeeValidator = employeeValidator;
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        _emailStore = (IUserEmailStore<Employee>)_userStore;
    }
    public async Task<IList<Employee>> GetAllAsync()
    {
        return await _context.Employees.Include(e => e.Position).ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _userStore.FindByIdAsync(id, CancellationToken.None);
    }

    public async Task<IdentityResult> CreateAsync(Employee employee, string password)
    {
        ValidationResult validationResult = await _employeeValidator.ValidateAsync(employee);

        if (!validationResult.IsValid)
        {
            return IdentityResult.Failed(new IdentityError());
        }

        if (employee.EmploymentStartDate >= employee.EmploymentEndDate)
        {
            return IdentityResult.Failed(new IdentityError());
        }

        if (employee.Position.Caption == Positions.HR)
        {
            await _userManager.AddToRoleAsync(employee, nameof(UserRoles.Administrator));
        }

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);
        IdentityResult result = await _userManager.CreateAsync(employee, password);

        return result;
    }

    public async Task<IdentityResult> UpdateAsync(Employee employee)
    {
        ValidationResult validationResult = await _employeeValidator.ValidateAsync(employee);

        if (!validationResult.IsValid)
        {
            return IdentityResult.Failed(new IdentityError());
        }
        
        if (employee.Position.Caption == Positions.HR)
        {
            await _userManager.AddToRoleAsync(employee, nameof(UserRoles.Administrator));
        }

        IdentityResult result = await _userManager.UpdateAsync(employee);

        return result;
    }

    public async Task<IdentityResult> SoftDeleteAsync(Employee employee)
    {
        employee.DeleteDate = DateTime.Now;
        await _userManager.SetLockoutEnabledAsync(employee, true);
        IdentityResult result = await _userManager.SetLockoutEndDateAsync(employee, DateTime.Today.AddYears(10));

        return result;
    }

    public async Task<List<Employee>> SearchAsync(string firstName, string lastName, DateTime? employmentStart, DateTime? employmentEnd)
    {
        var results = _context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(firstName))
        {
            results = results.Where(e => e.FirstName.ToLower().StartsWith(firstName));
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            results = results.Where(e => e.LastName.ToLower().StartsWith(lastName));
        }

        if (employmentStart is not null)
        {
            results = results.Where(e => e.EmploymentStartDate >= employmentStart);
        }

        if (employmentEnd is not null)
        {
            results = results.Where(e => e.EmploymentEndDate <= employmentEnd);
        }
        
        return await results.ToListAsync();
    }
}
