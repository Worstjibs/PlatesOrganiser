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

    public async Task<PlateUser?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.Include(x => x.Collections)
                                   .ThenInclude(x => x.Plates)
                                   .ThenInclude(x => x.PrimaryLabel)
                                   .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void AddUser(PlateUser user)
    {
        _context.Users.Add(user);
    }
}
