using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using PlatesOrganiser.Auth.Models;

namespace PlatesOrganiser.Auth.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var claims = await _userManager.GetClaimsAsync(user);

        context.AddRequestedClaims(claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;

        return Task.CompletedTask;
    }
}
