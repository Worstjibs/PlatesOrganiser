using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateCollectionRepository
{
    Task<PlateCollection> GetCollectionByIdAsync(int id);
    void AddCollection(PlateCollection collection);
}
