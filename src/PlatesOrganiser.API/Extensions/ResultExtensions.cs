using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.API.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result, string? location = null)
    {
        if (result.IsSuccess)
        {
            if (result.NewEntityId is not null && location is not null)
                return new CreatedResult($"{location}/{result.NewEntityId}", result.NewEntityId);

            return new NoContentResult();
        }

        ActionResult actionResult = result.Error switch
        {
            Error.Bad => new BadRequestResult(),
            Error.NotFound => new NotFoundResult(),
            _ => new BadRequestResult(),
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
