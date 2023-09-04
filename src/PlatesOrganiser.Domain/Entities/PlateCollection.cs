using PlatesOrganiser.Domain.Enum;

namespace PlatesOrganiser.Domain.Entities;

public class PlateCollection : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public CollectionType Type { get; set; } = CollectionType.Default;

    public Guid UserId { get; set; }
    public required PlateUser User { get; set; }

    public ICollection<Plate> Plates { get; set; } = new List<Plate>();
}
