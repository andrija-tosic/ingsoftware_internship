using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business;
using VacaYAY.Data.Models;

namespace VacaYAY.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeesController(ILogger<EmployeesController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{count}")]
    public IEnumerable<Employee> GetRandoms(int count)
    {
        return _unitOfWork.EmployeeService.GetRandoms(count);
    }
}
