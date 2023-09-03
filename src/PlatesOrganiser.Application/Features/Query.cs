using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features;

public abstract class Query<TResponse> : IRequest<Result<TResponse>>
{
}
