using System.Text;
using VacaYAY.Data;

namespace VacaYAY.Business.Jobs;

public class VacationJobs
{
    private readonly IUnitOfWork _unitOfWork;

    public VacationJobs(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task YearlyEnqueueEmailsForRemainingVacationDays()
    {
        const string subject = "Vacation days reminder";

        (var employees, var hrEmployees) = await _unitOfWork.EmployeeService.GetEmployeesWithRemainingVacationDaysAndAdmins();

        var hrEmailContent = new StringBuilder()
            .AppendLine("Employees with remaining vacation days:");

        int i = 1;
        foreach (var employee in employees)
        {
            hrEmailContent.AppendLine($"{i}) {employee.FirstName} {employee.LastName}: {employee.DaysOffNumber}");

            _unitOfWork.EmailService.EnqueueEmail(employee.Email!,
                subject,
                $"You have {employee.DaysOffNumber} days of vacation remaining");

            i++;
        }

        foreach (var hrEmployee in hrEmployees)
        {
            _unitOfWork.EmailService.EnqueueEmail(hrEmployee.Email!, subject, hrEmailContent.ToString());
        }
    }

    public async Task YearlyAddDaysToAllEmployees(int days = InitialData.YearlyVacationAddedDaysNumber)
    {
        await _unitOfWork.EmployeeService.AddDaysToAllEmployees(days);
    }

    public async Task YearlyRemoveLastYearsDaysOff()
    {
        await _unitOfWork.EmployeeService.RemoveLastYearsDaysOffFromAllEmployees();
    }
}
