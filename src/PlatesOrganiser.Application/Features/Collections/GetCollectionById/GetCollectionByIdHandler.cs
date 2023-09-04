using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Features.Collections.GetCollectionById;

public class GetCollectionByIdHandler : IQueryHandler<GetCollectionByIdQuery, CollectionDto>
{
    private readonly IPlateCollectionRepository _repository;

    public GetCollectionByIdHandler(IPlateCollectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CollectionDto>> Handle(GetCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetCollectionByIdAsync(request.Id, cancellationToken);

        if (collection is null)
            return Result.Failure<CollectionDto>(Error.NotFound, $"Collection with Id {request.Id} not found");

        return new CollectionDto
        {
            Id = collection.Id,
            Name = collection.Name
        };
    }
}
