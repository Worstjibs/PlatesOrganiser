using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

namespace PlatesOrganiser.API.Controllers;

public class PlatesController : BaseApiController
{
    private readonly IMediator _mediator;

    public PlatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PlateDto>> Create([FromBody] AddPlateCommand addPlateCommand, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(addPlateCommand, cancellationToken);

        return Ok(result);
    }
}
