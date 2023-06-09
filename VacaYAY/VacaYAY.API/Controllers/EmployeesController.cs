using Microsoft.AspNetCore.Mvc;
using VacaYAY.API.Fakes;
using VacaYAY.API.Models;

namespace VacaYAY.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger)
    {
        _logger = logger;
    }
    public IEnumerable<Employee> GenerateFakes(int count, IList<Position> positions)
    {
        return EmployeeFaker.GenerateFakes(count, positions);
    }

    [HttpGet("{count}")]
    public IEnumerable<Employee> GetFakesAsync(int count)
    {
        IList<Position> positions = new List<Position>() {
        new Position{ Id = 1, Caption = "HR", Description="Description", Employees = new List<Employee>() },
        new Position{ Id = 2, Caption="Senior iOS Developer", Description = "Description", Employees = new List<Employee>()},
        new Position{ Id = 3, Caption="Human Applications Representative", Description="Description", Employees = new List<Employee>()},
        new Position{ Id = 4, Caption="Product Accounts Director", Description="Description", Employees=new List<Employee>()}
        };
        return GenerateFakes(count, positions);
    }
}
