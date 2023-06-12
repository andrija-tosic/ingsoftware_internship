using Microsoft.AspNetCore.Mvc;
using VacaYAY.API.Fakes;
using VacaYAY.API.Models;

namespace VacaYAY.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    public EmployeesController()
    {
    }

    [HttpGet("{count}")]
    public IEnumerable<Employee> GetFakesAsync(int count)
    {
        var positions = new Position[]
        {
            new Position
            {
                Id = 1,
                Caption = "HR",
                Description = "Human Resources",
                Employees = new List<Employee>()
            },
            new Position {
                    Id = 2,
                    Caption = "iOS Developer",
                    Description = "Apple user",
                    Employees = new List<Employee>()
            },
            new Position
            {
                Id = 3,
                Caption = "Android Developer",
                Description = "Android user",
                Employees = new List<Employee>()
            },
            new Position
            {
                Id = 4,
                Caption = "MVC Intern",
                Description = "Lizard",
                Employees = new List<Employee>()
            },
            new Position
            {
                Id = 5,
                Caption = "Java Intern",
                Description = "Also lizard",
                Employees = new List<Employee>()
            }
        };

        return EmployeeFaker.GenerateFakes(count, positions);
    }
}
