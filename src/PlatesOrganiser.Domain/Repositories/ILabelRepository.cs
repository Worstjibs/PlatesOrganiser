using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Domain.Repositories;

public interface ILabelRepository
{
    Task<Label?> GetLabelByName(string name);
    Task<IEnumerable<Label>> GetLabelsByName(string[] names);
    void AddLabel(Label label);
}
