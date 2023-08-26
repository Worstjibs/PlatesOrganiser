using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Application.Services.CurrentUser;

public interface ICurrentUserService
{
    Task<PlateUser?> GetCurrentUser();
}
