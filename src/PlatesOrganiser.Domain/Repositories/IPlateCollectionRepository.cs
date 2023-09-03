using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateCollectionRepository
{
    Task<PlateCollection?> GetCollectionByIdAsync(Guid id, CancellationToken cancellationToken);
    void AddCollection(PlateCollection collection, CancellationToken cancellationToken);
}
