using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.API.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        ActionResult actionResult = result.Error switch
        {
            Error.Bad => new BadRequestResult(),
            Error.NotFound => new NotFoundResult(),
            _ => new NoContentResult(),
        };

        return actionResult;
    }

    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        ActionResult actionResult = result.Error switch
        {
            Error.Bad => new BadRequestObjectResult(result.Message),
            Error.NotFound => new NotFoundResult(),
            _ => new OkObjectResult(result.Value)
        };

        return actionResult;
    }
}
