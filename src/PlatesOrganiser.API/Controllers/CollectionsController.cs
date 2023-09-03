using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.API.Extensions;
using PlatesOrganiser.Application.Features.Collections.AddCollection;

namespace PlatesOrganiser.API.Controllers;

public class CollectionsController : BaseApiController
{
    private readonly IMediator _mediator;

    public CollectionsController(IMediator mediator, IValidator<AddCollectionCommand> validators)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddCollectionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult("collections");
    }
}
