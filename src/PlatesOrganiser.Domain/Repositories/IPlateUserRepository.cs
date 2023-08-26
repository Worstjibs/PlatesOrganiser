using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateUserRepository
{
    Task<PlateUser?> GetById(Guid id);
}
