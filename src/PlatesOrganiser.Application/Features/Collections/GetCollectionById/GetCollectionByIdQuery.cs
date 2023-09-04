namespace PlatesOrganiser.Application.Features.Collections.GetCollectionById;

public class GetCollectionByIdQuery : IQuery<CollectionDto>
{
    public Guid Id { get; set; }
}
