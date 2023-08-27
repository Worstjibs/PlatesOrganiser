using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateRepository
{
    Task<Plate?> GetPlateById(Guid id);
    Task<Plate?> GetPlateByMasterReleaseId(int id);
    Task<IEnumerable<Plate>> GetAllPlates();
    void AddPlate(Plate plate);
}
