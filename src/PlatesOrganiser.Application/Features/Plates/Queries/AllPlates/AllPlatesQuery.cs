using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Plates.Queries.AllPlates;

public class AllPlatesQuery : IRequest<Result<IEnumerable<PlateDto>>>
{
}
