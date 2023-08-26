using MediatR;
using PlatesOrganiser.API.Services;

public record GetAllQuery(string Title, string? Artist) : IRequest<IEnumerable<RecordQueryResponse>>;