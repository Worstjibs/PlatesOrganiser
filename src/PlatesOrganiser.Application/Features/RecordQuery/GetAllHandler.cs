using MediatR;
using PlatesOrganiser.API.Services;

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
        var response = await _discogsService.GetMasterRelease(request.Title, request.Artist);

        if (response == null)
            return Enumerable.Empty<RecordQueryResponse>();

        return new List<RecordQueryResponse> { response };
    }
}
