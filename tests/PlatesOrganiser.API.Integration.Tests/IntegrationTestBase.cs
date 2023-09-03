using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.API.Integration.Tests.Auth;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Infrastructure.Context;
using WireMock.Client;

namespace PlatesOrganiser.API.Integration.Tests;

[Collection("Shared collection")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly Random _random = new Random(808);

    protected readonly WebApiFactory _factory;
    protected readonly IWireMockAdminApi _wireMock;
    protected readonly HttpClient _client;

    public IntegrationTestBase(WebApiFactory factory)
    {
        _factory = factory;
        _wireMock = factory.WireMockApi;
        _client = factory.HttpClient;
    }

    public async Task DisposeAsync()
    {
        await _wireMock.ResetMappingsAsync();

        await _factory.ResetDbAsync();

        _client.DefaultRequestHeaders.Authorization = null;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    protected async Task<PlateUser> ActAsUser(Guid userId)
    {
        var user = new PlateUser { Id = userId, UserName = "TestUser" };

        _client.ActAsUser(userId);

        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<PlatesContext>();
        context.Users.Add(user);

        await context.SaveChangesAsync();

        return user;
    }
}
