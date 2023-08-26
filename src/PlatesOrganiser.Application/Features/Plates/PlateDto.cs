namespace PlatesOrganiser.Application.Features.Plates;

public class PlateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DiscogsMasterReleaseId { get; set; }
    public required PlateLabelDto PrimaryLabel { get; set; }
}