using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateUserRepository
{
    Task<PlateUser?> GetUserByIdAsync(Guid id);
    void AddUser(PlateUser user);
}
