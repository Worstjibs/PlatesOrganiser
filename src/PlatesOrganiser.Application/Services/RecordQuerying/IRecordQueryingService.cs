namespace PlatesOrganiser.API.RecordQuerying.Services;

public interface IRecordQueryingService
{
    public Task<RecordQueryResponse?> GetMasterReleaseById(int id);
    public Task<RecordQueryResponse?> GetMasterRelease(string title, string? artist = null, string? label = null);
}
