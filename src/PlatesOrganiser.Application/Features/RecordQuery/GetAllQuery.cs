using MediatR;
using PlatesOrganiser.API.RecordQuerying.Services;

public record GetAllQuery(string Title, string? Artist, string? Label) : IRequest<IEnumerable<RecordQueryResponse>>;