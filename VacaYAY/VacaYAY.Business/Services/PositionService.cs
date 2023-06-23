using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class PositionService : IPositionService
{
    private readonly VacayayDbContext _context;

    public PositionService(VacayayDbContext context)
    {
        _context = context;
    }
    public async Task<IList<Position>> GetAllAsync()
    {
        return await _context.Positions.ToListAsync();
    }

    public async Task<Position?> GetByIdAsync(int id)
    {
        return await _context.Positions.FindAsync(id);
    }
}
