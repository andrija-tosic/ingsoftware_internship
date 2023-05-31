using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IPositionService
{
    public Task<List<Position>> GetAllAsync();
    public Task<Position?> GetByIdAsync(int id);
}
