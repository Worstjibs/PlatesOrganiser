using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application;

public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
}
