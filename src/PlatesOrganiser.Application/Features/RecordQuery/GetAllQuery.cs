﻿using MediatR;
using PlatesOrganiser.API.Services;

public record GetAllQuery(string Title, string? Artist, string? Label) : IRequest<IEnumerable<RecordQueryResponse>>;