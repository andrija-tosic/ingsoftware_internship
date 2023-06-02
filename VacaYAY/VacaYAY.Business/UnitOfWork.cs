using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;
using VacaYAY.Data;
using VacaYAY.Business.Services;
using FluentValidation;

namespace VacaYAY.Business;

public class UnitOfWork : IUnitOfWork
{
    private readonly VacayayDbContext _context;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IUserStore<Employee> _userStore;
    private readonly UserManager<Employee> _userManager;
    private readonly IValidator<Employee> _employeeValidator;

    public UnitOfWork(VacayayDbContext context, IUserStore<Employee> userStore, UserManager<Employee> userManager, IValidator<Employee> employeeValidator)
    {
        _context = context;
        _userStore = userStore;
        _userManager = userManager;
        _employeeValidator = employeeValidator;
        _employeeService = new EmployeeService(_context, _userStore, _userManager, _employeeValidator);
        _positionService  = new PositionService(_context);
    }

    public IEmployeeService EmployeeService => _employeeService;

    public IPositionService PositionService => _positionService;

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
