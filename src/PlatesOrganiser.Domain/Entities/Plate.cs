namespace PlatesOrganiser.Domain.Entities;

public class Plate : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DiscogsMasterReleaseId { get; set; }

    public Guid PrimaryLabelId { get; set; }
    public required Label PrimaryLabel { get; set; }
}
