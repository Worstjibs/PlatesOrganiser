using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.API.RecordQuerying.Services;

namespace PlatesOrganiser.API.Controllers;

public class RecordQueryController : BaseApiController
{
    private readonly IMediator _mediator;

    public RecordQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecordQueryResponse>>> GetAllForTitle([FromQuery] GetAllQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
