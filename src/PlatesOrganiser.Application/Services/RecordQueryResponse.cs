namespace PlatesOrganiser.API.Services;

public class RecordQueryResponse
{
    public RecordQueryResponse(int masterReleaseId, string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        MasterReleaseId = masterReleaseId;
        Title = title;
    }

    public int MasterReleaseId { get; set; }
    public string Title { get; init; }
    public ushort Year { get; init; }
    public string? PrimaryLabel { get; init; }
    public List<string> Artists { get; init; } = new List<string>();
    public List<string> OtherLabels { get; init; } = new List<string>();
}
