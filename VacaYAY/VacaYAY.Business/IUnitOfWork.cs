using VacaYAY.Business.Services;

namespace VacaYAY.Business;

public interface IUnitOfWork
{
    public IEmployeeService EmployeeService { get; }
    public IPositionService PositionService { get; }
    public IEmailService EmailService { get; }
    public Task<int> SaveChangesAsync();
}
