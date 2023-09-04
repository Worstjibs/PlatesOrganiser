using MediatR;
using PlatesOrganiser.API.RecordQuerying.Services;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Enum;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

internal class AddPlateHandler : ICommandHandler<AddPlateCommand>
{
    private readonly IRecordQueryingService _queryingService;
    private readonly ILabelRepository _labelRepository;
    private readonly IPlateRepository _plateRepository;
    private readonly IPlateUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AddPlateHandler(
        IRecordQueryingService queryingService,
        ILabelRepository labelRepository,
        IPlateRepository plateRepository,
        IPlateUserRepository userRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork
    )
    {
        _queryingService = queryingService;
        _labelRepository = labelRepository;
        _plateRepository = plateRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPlateCommand request, CancellationToken cancellationToken)
    {
        var plate = await _plateRepository.GetPlateByMasterReleaseIdAsync(request.MasterReleaseId);
        plate ??= await QueryForMasterRelease(request.MasterReleaseId);

        if (plate is null)
            return Result.Failure<PlateDto>(Error.NotFound, $"Plate with master Id {request.MasterReleaseId} not found.");

        var user = await _currentUserService.GetCurrentUserAsync();
        if (user is null)
        {
            user = _currentUserService.CreateUserFromClaims();
            _userRepository.AddUser(user);
        }

        if (!user.Collections.Any())
            user.Collections.Add(new PlateCollection { Name = "Default", User = user });

        var defaultCollection = user.Collections.First(x => x.Type == CollectionType.Default);
        defaultCollection.Plates.Add(plate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(plate.Id);
    }

    private async Task<Plate?> QueryForMasterRelease(int masterReleaseId)
    {
        var masterRelease = await _queryingService.GetMasterReleaseByIdAsync(masterReleaseId);
        if (masterRelease is null)
            return null;

        var label = await GetOrCreateLabel(masterRelease.PrimaryLabel!);

        var plate = new Plate
        {
            Name = masterRelease.Title,
            DiscogsMasterReleaseId = masterRelease.ReleaseId,
            PrimaryLabelId = label.Id,
            PrimaryLabel = label
        };

        _plateRepository.AddPlate(plate);

        return plate;
    }

    private async Task<Label> GetOrCreateLabel(string labelName)
    {
        var label = await _labelRepository.GetLabelByNameAsync(labelName);
        if (label is not null)
            return label;

        label = new Label(labelName);

        return label;
    }
}
