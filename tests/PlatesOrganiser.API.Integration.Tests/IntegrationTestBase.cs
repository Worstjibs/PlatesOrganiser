using WireMock.Client;

namespace PlatesOrganiser.API.Integration.Tests;

[Collection("Shared collection")]
public class IntegrationTestBase : IAsyncLifetime
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
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
