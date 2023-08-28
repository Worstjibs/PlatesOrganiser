using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using ParkSquare.Discogs;
using ParkSquare.Discogs.Dto;
using PlatesOrganiser.Infrastructure.Context;
using PlatesOrganiser.Infrastructure.Services;
using System.Security.Cryptography;
using Testcontainers.PostgreSql;
using WireMock.Client;
using WireMock.Client.Extensions;
using WireMock.Net.Testcontainers;
using WireMock.Server;

namespace PlatesOrganiser.API.Integration.Tests;

public class WebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly WireMockContainer _mockServer;

    public WebApiFactory()
    {
        _dbContainer = new PostgreSqlBuilder().Build();

        _mockServer = new WireMockContainerBuilder()
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(config =>
        {
            config.RemoveAll<PlatesContext>();
            config.RemoveAll<DbContextOptions<PlatesContext>>();

            config.RemoveAll<IClientConfig>();
            config.AddSingleton<IClientConfig>(_ => new ClientConfig
            {
                AuthToken = string.Empty,
                BaseUrl = _mockServer.GetPublicUrl()
            });

            config.RemoveAll<IDiscogsClient>();
            config.AddHttpClient<IDiscogsClient, DiscogsClient>(_ => _mockServer.CreateClient());

            config.AddDbContext<PlatesContext>(config => config.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public HttpClient HttpClient { get; private set; } = default!;
    public IWireMockAdminApi WireMockApi { get; set; }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await _mockServer.StartAsync();

        WireMockApi = _mockServer.CreateWireMockAdminClient();

        HttpClient = CreateClient();
    }
    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
