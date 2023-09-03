using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features;

public interface ICommand : IRequest<Result>
{
}
