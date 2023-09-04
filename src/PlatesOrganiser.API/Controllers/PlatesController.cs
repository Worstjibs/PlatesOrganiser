using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.API.Extensions;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;
using PlatesOrganiser.Application.Features.Plates.Queries.AllPlates;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.API.Controllers;

public class PlatesController : BaseApiController
{
    private readonly IMediator _mediator;

    public PlatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlateDto>>> All([FromQuery] AllPlatesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Result<PlateDto>>> Create([FromBody] AddPlateCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult("plates");
    }
}
