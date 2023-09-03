using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}
