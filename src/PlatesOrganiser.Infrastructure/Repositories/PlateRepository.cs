using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

public class PlateRepository : IPlateRepository
{
    private readonly PlatesContext _context;

    public PlateRepository(PlatesContext context)
    {
        _context = context;
    }

    public void AddPlate(Plate plate)
    {
        _context.Plates.Add(plate);
    }

    public async Task<Plate?> GetPlateById(Guid id)
    {
        return await _context.Plates
                                .Include(x => x.PrimaryLabel)
                                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Plate?> GetPlateByMasterReleaseId(int id)
    {
        return await _context.Plates
                                .Include(x => x.PrimaryLabel)
                                .FirstOrDefaultAsync(x => x.DiscogsMasterReleaseId == id);
    }
}
