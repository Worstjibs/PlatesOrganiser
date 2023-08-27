using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

public record AddPlateCommand(int MasterReleaseId) : IRequest<Result<PlateDto>>;
