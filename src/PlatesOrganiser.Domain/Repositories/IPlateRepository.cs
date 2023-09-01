using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface IPlateRepository
{
    Task<Plate?> GetPlateByIdAsync(Guid id);
    Task<Plate?> GetPlateByMasterReleaseIdAsync(int id);
    Task<IEnumerable<Plate>> GetAllPlatesAsync();
    void AddPlate(Plate plate);
}
