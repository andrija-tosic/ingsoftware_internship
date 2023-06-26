using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;
using VacaYAY.Data;
using VacaYAY.Business.Services;
using VacaYAY.Business.Validators;
using Microsoft.Extensions.Logging;
using SendGrid;

namespace VacaYAY.Business;

public class UnitOfWork : IUnitOfWork
{
    private readonly VacayayDbContext _context;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IVacationService _vacationService;
    private readonly IContractService _contractService;
    private readonly IFileService _fileService;
    private readonly IUserStore<Employee> _userStore;
    private readonly UserManager<Employee> _userManager;
    private readonly IEmailService _emailService;

    public UnitOfWork(VacayayDbContext context,
        IUserStore<Employee> userStore,
        UserManager<Employee> userManager,
        IHttpService httpService,
        ISendGridClient sendGridClient,
        ILogger<IVacationService> vacationLogger,
        ILogger<IContractService> contractLogger)
    {
        _context = context;
        _userStore = userStore;
        _userManager = userManager;
        _employeeService ??= new EmployeeService(_context, _userStore, _userManager, new EmployeeValidator(), httpService);
        _positionService ??= new PositionService(_context);
        _vacationService ??= new VacationService(_context, new VacationRequestValidator(this), vacationLogger);
        _emailService ??= new EmailService(sendGridClient);
        _contractService ??= new ContractService(_context.Contracts, _context.ContractTypes, contractLogger);
        _fileService ??= new FileService("UseDevelopmentStorage=true");
    }

    public IEmployeeService EmployeeService => _employeeService;
    public IPositionService PositionService => _positionService;
    public IVacationService VacationService => _vacationService;
    public IEmailService EmailService => _emailService;
    public IContractService ContractService => _contractService;
    public IFileService FileService => _fileService;
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
