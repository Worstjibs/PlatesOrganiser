using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

internal class PlateRepository : IPlateRepository
{
    private readonly PlatesContext _context;

    public PlateRepository(PlatesContext context)
    {
        _context = context;
    }

    public async Task<Plate?> GetPlateByIdAsync(Guid id)
    {
        return await _context.Plates
                                .Include(x => x.PrimaryLabel)
                                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Plate?> GetPlateByMasterReleaseIdAsync(int id)
    {
        return await _context.Plates
                                .Include(x => x.PrimaryLabel)
                                .FirstOrDefaultAsync(x => x.DiscogsMasterReleaseId == id);
    }

    public async Task<IEnumerable<Plate>> GetAllPlatesAsync()
    {
        return await _context.Plates
                                .Include(x => x.PrimaryLabel)
                                .ToListAsync();
    }

    public void AddPlate(Plate plate)
    {
        _context.Plates.Add(plate);
    }
}
