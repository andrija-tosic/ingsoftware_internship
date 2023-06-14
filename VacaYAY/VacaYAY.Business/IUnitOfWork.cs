using VacaYAY.Business.Services;

namespace VacaYAY.Business;

public interface IUnitOfWork
{
    public IEmployeeService EmployeeService { get; }
    public IPositionService PositionService { get; }
    public IVacationService VacationService { get; }
    public Task<int> SaveChangesAsync();
}
