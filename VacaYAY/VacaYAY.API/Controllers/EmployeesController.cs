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
    public async Task<IEnumerable<Employee>> GetFakesAsync(int count)
    {
        IList<Position> positions = (await _unitOfWork.PositionService.GetAllAsync()).ToList();
        return _unitOfWork.EmployeeService.GenerateFakes(count, positions);
    }
}
