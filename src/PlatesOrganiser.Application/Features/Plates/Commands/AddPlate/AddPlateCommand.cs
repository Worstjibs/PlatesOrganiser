using MediatR;

namespace PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

public record AddPlateCommand(int MasterReleaseId) : IRequest<PlateDto>;
