using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IHttpService
{
    public Task<IList<Employee>?> GetFakeEmployees(int count);
}
