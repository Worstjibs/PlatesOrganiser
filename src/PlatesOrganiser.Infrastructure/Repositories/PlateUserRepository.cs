using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

internal class PlateUserRepository : IPlateUserRepository
{
    private readonly PlatesContext _context;

    public PlateUserRepository(PlatesContext context)
    {
        _context = context;
    }

    public async Task<PlateUser?> GetById(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
