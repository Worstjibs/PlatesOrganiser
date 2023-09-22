using MediatR;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Collections.Commands.AddCollection;

internal class AddCollectionHandler : IRequestHandler<AddCollectionCommand, Result>
{
    private readonly IPlateCollectionRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AddCollectionHandler(IPlateCollectionRepository repository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddCollectionCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        if (user is null)
            return Result.Failure(Error.Bad, "User not found.");

        if (await _repository.CollectionExistsAsync(request.Name, user.Id))
            return Result.Failure(Error.Bad, $"Collection with name {request.Name} already exists");

        var plateCollection = new PlateCollection { Name = request.Name, User = user };

        _repository.AddCollection(plateCollection, cancellationToken);

        if (await _unitOfWork.SaveChangesAsync(cancellationToken))
            return Result.Success(newEntityId: plateCollection.Id);

        return Result.Failure<Result>(Error.Unknown, "Something went wrong saving the collection to the database");
    }
}
