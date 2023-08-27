using MediatR;
using PlatesOrganiser.API.Services;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

internal class AddPlateHandler : IRequestHandler<AddPlateCommand, Result<PlateDto>>
{
    private readonly IRecordQueryingService _queryingService;
    private readonly ILabelRepository _labelRepository;
    private readonly IPlateRepository _plateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPlateHandler(
        IRecordQueryingService queryingService,
        ILabelRepository labelRepository,
        IPlateRepository plateRepository,
        IUnitOfWork unitOfWork
    )
    {
        _queryingService = queryingService;
        _labelRepository = labelRepository;
        _plateRepository = plateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PlateDto>> Handle(AddPlateCommand request, CancellationToken cancellationToken)
    {
        var plate = await _plateRepository.GetPlateByMasterReleaseId(request.MasterReleaseId);
        if (plate is not null)
            return Result.Failure<PlateDto>(Error.Bad);

        var masterRelease = await _queryingService.GetMasterReleaseById(request.MasterReleaseId);
        if (masterRelease is null)
            return Result.Failure<PlateDto>(Error.NotFound);

        var label = await GetOrCreateLabel(masterRelease.PrimaryLabel!);

        plate = new Plate
        {
            Name = masterRelease.Title,
            DiscogsMasterReleaseId = masterRelease.MasterReleaseId,
            PrimaryLabelId = label.Id,
            PrimaryLabel = label
        };

        _plateRepository.AddPlate(plate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create(MapToDto(plate));
    }

    private PlateDto MapToDto(Plate plate)
    {
        return new PlateDto
        {
            Id = plate.Id,
            Name = plate.Name,
            DiscogsMasterReleaseId = plate.DiscogsMasterReleaseId,
            PrimaryLabel = new PlateLabelDto
            {
                Id = plate.PrimaryLabel.Id,
                Name = plate.PrimaryLabel.Name
            }
        };
    }

    private async Task<Label> GetOrCreateLabel(string labelName)
    {
        var label = await _labelRepository.GetLabelByName(labelName);
        if (label is not null)
            return label;

        label = new Label(labelName);

        return label;
    }
}
