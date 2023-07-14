using System.Text;
using VacaYAY.Business.Services;
using VacaYAY.Data;

namespace VacaYAY.Business.Jobs;

public class VacationJobs
{
    private readonly IEmployeeService _employeeService;
    private readonly IEmailService _emailService;

    public VacationJobs(
        IEmployeeService employeeService,
        IEmailService emailService)
    {
        _employeeService = employeeService;
        _emailService = emailService;
    }
    public async Task YearlyEnqueueEmailsForRemainingVacationDays()
    {
        const string subject = "Vacation days reminder";

        (var employees, var hrEmployees) = await _employeeService.GetEmployeesWithRemainingVacationDaysAndAdmins();

        var hrEmailContent = new StringBuilder()
            .AppendLine("Employees with remaining vacation days:");

        int i = 1;
        foreach (var employee in employees)
        {
            hrEmailContent.AppendLine($"{i}) {employee.FirstName} {employee.LastName}: {employee.DaysOffNumber}");

            _emailService.EnqueueEmail(employee.Email!,
                subject,
                $"You have {employee.DaysOffNumber} days of vacation remaining");

            i++;
        }

        foreach (var hrEmployee in hrEmployees)
        {
            _emailService.EnqueueEmail(hrEmployee.Email!, subject, hrEmailContent.ToString());
        }
    }

    public async Task YearlyAddDaysToAllEmployees(int days = InitialData.YearlyVacationAddedDaysNumber)
    {
        await _employeeService.AddDaysToAllEmployees(days);
    }

    public async Task YearlyRemoveLastYearsDaysOff()
    {
        await _employeeService.RemoveLastYearsDaysOffFromAllEmployees();
    }
}
