using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
