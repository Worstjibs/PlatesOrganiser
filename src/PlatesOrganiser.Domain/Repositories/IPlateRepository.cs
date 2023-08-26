using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateRepository
{
    void AddPlate(Plate plate);
    Task<Plate?> GetPlateByMasterReleaseId(int id);
    Task<Plate?> GetPlateById(Guid id);
}
