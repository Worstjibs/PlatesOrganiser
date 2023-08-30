using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace PlatesOrganiser.API.Integration.Tests.Auth;

public static class HttpClientExtensions
{
    public static void ActAsUser(this HttpClient client, Guid userId)
    {
        var subjectClaim = new Claim(JwtClaimTypes.Subject, userId.ToString());
        var userName = new Claim(JwtClaimTypes.PreferredUserName, "test_user");

        var jwt = MockJwtTokens.GenerateJwtToken(new[] { subjectClaim, userName });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwt);
    }
}
