using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

internal class PlateCollectionRepository : IPlateCollectionRepository
{
    private readonly PlatesContext _context;

    public PlateCollectionRepository(PlatesContext context)
    {
        _context = context;
    }

    public async Task<PlateCollection?> GetCollectionByIdAsync(Guid id, CancellationToken cancellationToken)
        =>
            await _context.Collections
                .Include(x => x.Plates)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void AddCollection(PlateCollection collection, CancellationToken cancellationToken)
        =>
            _context.Collections.Add(collection);
}
