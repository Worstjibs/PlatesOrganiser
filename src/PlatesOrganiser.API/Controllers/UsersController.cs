using Microsoft.AspNetCore.Mvc;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.API.Controllers;

public class UsersController : BaseApiController
{
    private readonly ICurrentUserService _currentUserService;

    public UsersController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<PlateUser?>> GetCurrentUser()
    {
        var result = await _currentUserService.GetCurrentUserAsync();

        return Ok(result);
    }
}
