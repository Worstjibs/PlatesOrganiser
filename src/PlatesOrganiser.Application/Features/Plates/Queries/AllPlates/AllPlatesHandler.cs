using MediatR;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Plates.Queries.AllPlates;

public class AllPlatesHandler : IRequestHandler<AllPlatesQuery, Result<IEnumerable<PlateDto>>>
{
    private readonly IPlateRepository _plateRepository;

    public AllPlatesHandler(IPlateRepository plateRepository)
    {
        _plateRepository = plateRepository;
    }

    public async Task<Result<IEnumerable<PlateDto>>> Handle(AllPlatesQuery request, CancellationToken cancellationToken)
    {
        var plates = await _plateRepository.GetAllPlatesAsync();

        var dtos = plates.Select(x =>
                            new PlateDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                DiscogsMasterReleaseId = x.DiscogsMasterReleaseId,
                                PrimaryLabel = new PlateLabelDto
                                {
                                    Id = x.PrimaryLabel.Id,
                                    Name = x.PrimaryLabel.Name
                                }
                            }).ToArray();

        return dtos;
    }
}
