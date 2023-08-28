namespace PlatesOrganiser.API.RecordQuerying.Services;

public class RecordQueryResponse
{
    public RecordQueryResponse(int releaseId, string title, bool isMasterRelease)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        ReleaseId = releaseId;
        Title = title;
    }

    public int ReleaseId { get; init; }
    public string Title { get; init; }
    public ushort Year { get; init; }
    public string? PrimaryLabel { get; init; }
    public List<string> Artists { get; init; } = new List<string>();
    public List<string> OtherLabels { get; init; } = new List<string>();
}
