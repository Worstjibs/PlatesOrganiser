using PlatesOrganiser.API.Seed;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using System.Security.Claims;

namespace PlatesOrganiser.API.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IPlateUserRepository _plateUserRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IPlateUserRepository plateUserRepository, IHttpContextAccessor httpContextAccessor)
    {
        _plateUserRepository = plateUserRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PlateUser?> GetCurrentUser()
    {
        var dbUser = await _plateUserRepository.GetById(Seeder.DummyUserGuid);

        return dbUser;
    }
}
