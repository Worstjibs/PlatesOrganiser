using Microsoft.AspNetCore.Mvc.Testing;

namespace PlatesOrganiser.API.Integration.Tests;

public class WebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;

    public Task InitializeAsync()
    {
        HttpClient = CreateClient();
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
