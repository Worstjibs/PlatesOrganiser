using IdentityModel;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;

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

    public async Task<PlateUser?> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext!.User.Claims.First(x => x.Type == JwtClaimTypes.Subject);

        var dbUser = await _plateUserRepository.GetUserByIdAsync(Guid.Parse(userId.Value));

        return dbUser;
    }

    public PlateUser CreateUserFromClaims()
    {
        var userClaims = _httpContextAccessor.HttpContext!.User.Claims;

        var userId = userClaims.First(x => x.Type == JwtClaimTypes.Subject);
        var userName = userClaims.First(x => x.Type == JwtClaimTypes.PreferredUserName);

        return new PlateUser { Id = Guid.Parse(userId.Value), UserName = userName.Value };
    }
}
