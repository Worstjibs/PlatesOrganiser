using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface ILabelRepository
{
    Task<Label?> GetLabelByNameAsync(string name);
    Task<IEnumerable<Label>> GetLabelsByNameAsync(string[] names);
    void AddLabel(Label label);
}
