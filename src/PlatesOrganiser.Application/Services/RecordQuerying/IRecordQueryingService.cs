namespace PlatesOrganiser.API.RecordQuerying.Services;

public interface IRecordQueryingService
{
    public Task<RecordQueryResponse?> GetMasterReleaseByIdAsync(int masterReleaseId);
    public Task<RecordQueryResponse?> GetReleaseAsync(string title, string? artist = null, string? label = null);
}
