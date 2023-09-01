namespace PlatesOrganiser.Domain.Entities;

public class PlateUser : IEntity, IAuditable
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public required string UserName { get; set; }
    public ICollection<PlateCollection> Collections { get; set; } = new List<PlateCollection>();
}
