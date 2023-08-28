using MediatR;
using PlatesOrganiser.API.RecordQuerying.Services;

namespace PlatesOrganiser.Application.Features.Discogs;

internal class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<RecordQueryResponse>>
{
    private readonly IRecordQueryingService _discogsService;

    public GetAllHandler(IRecordQueryingService discogsService)
    {
        _discogsService = discogsService;
    }

    public async Task<IEnumerable<RecordQueryResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var response = await _discogsService.GetMasterRelease(request.Title, request.Artist, request.Label);

        if (response == null)
            return Enumerable.Empty<RecordQueryResponse>();

        return new List<RecordQueryResponse> { response };
    }
}
