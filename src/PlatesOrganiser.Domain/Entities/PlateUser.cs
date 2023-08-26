namespace PlatesOrganiser.Domain.Entities;

public class PlateUser : IEntity
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public ICollection<Plate> Plates { get; set; } = new List<Plate>();
}
