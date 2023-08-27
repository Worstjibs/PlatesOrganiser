using ParkSquare.Discogs;
using PlatesOrganiser.API.Services;

namespace PlatesOrganiser.Infrastructure.Services;

public class RecordQueryingService : IRecordQueryingService
{
    private readonly IDiscogsClient _discogsClient;

    public RecordQueryingService(IDiscogsClient discogsClient)
    {
        _discogsClient = discogsClient;
    }

    public async Task<RecordQueryResponse?> GetMasterReleaseById(int id)
    {
        var masterRelease = await GetMasterReleaseInternal(id);

        return masterRelease;
    }

    public async Task<RecordQueryResponse?> GetMasterRelease(string title, string? artist, string? label)
    {
        var searchResult = await _discogsClient.SearchAsync(new SearchCriteria
        {
            Title = title,
            Artist = artist,
            Label = label
        });

        var masterReleases = searchResult.Results
                                    .Where(x => x.MasterId is not null)
                                    .GroupBy(x => x.MasterId, x => x, (g, k) => new { Count = k.Count(), Id = g })
                                    .OrderByDescending(x => x.Count)
                                    .ToList();


        var masterId = masterReleases.FirstOrDefault()?.Id;
        if (masterId is null)
            return null;

        return await GetMasterReleaseInternal(masterId.Value);
    }

    private async Task<RecordQueryResponse?> GetMasterReleaseInternal(int masterReleaseId)
    {
        try
        {
            var masterRelease = await _discogsClient.GetMasterReleaseAsync(masterReleaseId);
            if (masterRelease is null)
                return null;

            var allVersions = await _discogsClient.GetAllVersionsAsync(new VersionsCriteria(masterRelease.MasterId));

            var labels = allVersions.Versions
                                        .Select(x => x.Label)
                                        .GroupBy(x => x, x => x, (g, k) => new { Count = k.Count(), Name = g })
                                        .OrderByDescending(x => x.Count)
                                        .ToList();

            var primaryLabel = labels.FirstOrDefault()?.Name;

            var response = new RecordQueryResponse(
                masterReleaseId,
                masterRelease.Title)
            {
                Year = (ushort)masterRelease.Year,
                PrimaryLabel = primaryLabel,
                OtherLabels = labels.Select(x => x.Name).Distinct().ToList(),
                Artists = masterRelease.Artists.Select(x => x.Name).ToList()
            };

            return response;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
