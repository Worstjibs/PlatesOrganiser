using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly PlatesContext _context;

    public UnitOfWork(PlatesContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
