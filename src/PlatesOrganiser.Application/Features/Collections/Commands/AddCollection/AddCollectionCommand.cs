using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Collections.Commands.AddCollection;

public record AddCollectionCommand(string Name) : IRequest<Result>;