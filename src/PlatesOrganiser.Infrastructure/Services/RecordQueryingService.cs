using ParkSquare.Discogs;
using PlatesOrganiser.API.RecordQuerying.Services;
using System.Net;

namespace PlatesOrganiser.Infrastructure.Services;

public class RecordQueryingService : IRecordQueryingService
{
    private readonly IDiscogsClient _discogsClient;

    public RecordQueryingService(IDiscogsClient discogsClient)
    {
        _discogsClient = discogsClient;
    }

    public async Task<RecordQueryResponse?> GetMasterReleaseByIdAsync(int masterReleaseId)
    {
        var masterRelease = await GetMasterReleaseInternal(masterReleaseId);

        return masterRelease;
    }

    public async Task<RecordQueryResponse?> GetReleaseAsync(string title, string? artist, string? label)
    {
        var searchResult = await _discogsClient.SearchAsync(new SearchCriteria
        {
            Title = title,
            Artist = artist,
            Label = label
        });

        var groupedReleases = searchResult.Results
                                    .GroupBy(
                                        s => new { s.Title, s.MasterId },
                                        s => s,
                                        (g, k) => new { Key = g, Count = k.Count(), Items = k })
                                    .OrderByDescending(x => x.Count)
                                    .ToList();

        var firstGroup = groupedReleases.FirstOrDefault();
        if (firstGroup is null)
            return null;

        if (firstGroup.Key.MasterId is not null)
            return await GetMasterReleaseInternal(firstGroup.Key.MasterId.Value);

        var releaseId = firstGroup.Items.First().ReleaseId;
        var release = await _discogsClient.GetReleaseAsync(releaseId);

        var labelNames = release.Labels.Select(x => x.Name).Distinct().ToList();

        return new RecordQueryResponse(
            release.ReleaseId,
            release.Title,
            false)
        {
            Year = (ushort)release.Year,
            PrimaryLabel = labelNames.FirstOrDefault(),
            OtherLabels = labelNames,
            Artists = release.Artists.Select(x => x.Name).ToList()
        };
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
                masterRelease.Title,
                true)
            {
                Year = (ushort)masterRelease.Year,
                PrimaryLabel = primaryLabel,
                OtherLabels = labels.Select(x => x.Name).Distinct().ToList(),
                Artists = masterRelease.Artists.Select(x => x.Name).ToList()
            };

            return response;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}
