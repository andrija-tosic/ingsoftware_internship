using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public interface IPositionService
{
    public Task<List<Position>> GetPositions();
    public Task<Position?> GetById(int id);
}
