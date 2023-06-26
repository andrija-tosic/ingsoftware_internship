using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class EmployeeService : IEmployeeService
{
    private readonly VacayayDbContext _context;
    private readonly IUserStore<Employee> _userStore;
    private readonly UserManager<Employee> _userManager;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly IHttpService _httpService;
    private readonly IUserEmailStore<Employee> _emailStore;

    public EmployeeService(VacayayDbContext context, IUserStore<Employee> userStore, UserManager<Employee> userManager,
        IValidator<Employee> employeeValidator, IHttpService httpService)
    {
        _context = context;
        _userStore = userStore;
        _userManager = userManager;
        _employeeValidator = employeeValidator;
        _httpService = httpService;
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

    public async Task<Employee?> GetLoggedInAsync(ClaimsPrincipal claims)
    {
        var employee = await _userManager.GetUserAsync(claims);

        if (employee is null)
        {
            return null;
        }

        return await GetByIdAsync(employee.Id);
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _context.Employees
            .Where(e => e.Id == id)
            .Include(e => e.Position)
            .Include(e => e.Contracts).ThenInclude(c => c.Type)
            .SingleAsync();
    }

    public async Task<ValidationResult> CreateAsync(Employee employee, string password)
    {
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        if (!validationResult.IsValid)
        {
            return validationResult;
        }

        bool isAlreadyAdmin = await _userManager.IsInRoleAsync(employee, InitialData.AdminRoleName);

        if (employee.Position.Id == InitialData.AdminPosition.Id && !isAlreadyAdmin)
        {
            await _userManager.AddToRoleAsync(employee, InitialData.AdminRoleName);
        }

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);
        IdentityResult result = await _userManager.CreateAsync(employee, password);

        validationResult.Errors.AddRange(
            result.Errors
            .Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description))
        );

        return validationResult;
    }

    public async Task<ValidationResult> UpdateAsync(Employee employee)
    {
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        if (!validationResult.IsValid)
        {
            return validationResult;
        }

        if (employee.Position.Id == InitialData.AdminPosition.Id)
        {
            await _userManager.AddToRoleAsync(employee, InitialData.AdminRoleName);
        }

        IdentityResult result = await _userManager.UpdateAsync(employee);

        return validationResult;
    }

    public async Task<IdentityResult> SoftDeleteAsync(Employee employee)
    {
        employee.DeleteDate = DateTime.Now.Date;
        await _userManager.SetLockoutEnabledAsync(employee, true);
        IdentityResult result = await _userManager.SetLockoutEndDateAsync(employee, DateTime.Today.AddYears(10));

        return result;
    }

    public async Task<IList<Employee>> SearchAsync(EmployeeSearchFilters searchFilters)
    {
        IQueryable<Employee> employees = _context.Employees
            .Include(e => e.Position)
            .AsQueryable();

        IQueryable<Employee> nameResults = employees.Where(e => false);

        if (!string.IsNullOrWhiteSpace(searchFilters.EmployeeFullName))
        {
            foreach (string token in searchFilters.EmployeeFullName!.Trim().Split(" "))
            {
                nameResults = nameResults.Union(employees.Where(v => v.FirstName.Contains(token) || v.LastName.Contains(token)));
            }
        }

        if (searchFilters.EmploymentStartDate is not null)
        {
            employees = employees.Where(e => e.EmploymentStartDate >= searchFilters.EmploymentStartDate);
        }

        if (searchFilters.EmploymentEndDate is not null)
        {
            employees = employees.Where(e => e.EmploymentEndDate <= searchFilters.EmploymentEndDate);
        }

        if (searchFilters.PositionIds?.Length > 0)
        {
            employees = employees.Where(e => searchFilters.PositionIds.Contains(e.Position.Id));
        }

        if (nameResults.Any())
        {
            return await employees.Intersect(nameResults).ToListAsync();
        }
        else
        {
            return await employees.ToListAsync();
        }
    }

    public async Task<ValidationResult> CreateFakesAsync(int count)
    {
        IList<Employee>? employees = await _httpService.GetAsync<IList<Employee>>($"/Employees/{count}");

        if (employees is null || employees.Count == 0)
        {
            return new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure(nameof(employees), "is null") });
        }

        ValidationResult result = new ValidationResult();

        foreach (var employee in employees!)
        {
            Position? position = await _context.Positions.FindAsync(employee.Position.Id); // To avoid adding an already existing Position.
            if (position is null)
            {
                return new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure(nameof(position), "is null") });
            }
            employee.Position = position;
            result = await CreateAsync(employee, "password");
        }

        return result;
    }
    public async Task<bool> IsInRoleAsync(Employee employee, string role)
    {
        return await _userManager.IsInRoleAsync(employee, role);
    }

    public async Task<IList<Employee>> GetByPositions(int[] positions)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Where(e => positions.Contains(e.Position.Id))
            .ToListAsync();
    }
}
