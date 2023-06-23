using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IPositionService
{
    public Task<IList<Position>> GetAllAsync();
    public Task<Position?> GetByIdAsync(int id);
}
