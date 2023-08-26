using PlatesOrganiser.API.Seed;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;

namespace PlatesOrganiser.API.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IPlateUserRepository _plateUserRepository;

    public CurrentUserService(IPlateUserRepository plateUserRepository)
    {
        _plateUserRepository = plateUserRepository;
    }

    public async Task<PlateUser?> GetCurrentUser()
    {
        var user = await _plateUserRepository.GetById(Seeder.DummyUserGuid);

        return user;
    }
}
