namespace PlatesOrganiser.Application.Features.Collections.Queries.GetCollectionById;

public class GetCollectionByIdQuery : IQuery<CollectionDto>
{
    public Guid Id { get; set; }
}
