using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ParkSquare.Discogs;
using PlatesOrganiser.API.Integration.Tests.Auth;
using PlatesOrganiser.Infrastructure.Context;
using PlatesOrganiser.Infrastructure.Services;
using Testcontainers.PostgreSql;
using WireMock.Client;
using WireMock.Net.Testcontainers;

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
        builder.ConfigureTestServices(services =>
        {
            ReplaceDatabase(services);
            ReconfigureDiscogs(services);

            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var config = new OpenIdConnectConfiguration()
                {
                    Issuer = MockJwtTokens.Issuer
                };

                config.SigningKeys.Add(MockJwtTokens.SecurityKey);
                options.Configuration = config;
            });
        });
    }

    private void ReplaceDatabase(IServiceCollection services)
    {
        services.RemoveAll<PlatesContext>();
        services.RemoveAll<DbContextOptions<PlatesContext>>();

        services.AddDbContext<PlatesContext>(config => config.UseNpgsql(_dbContainer.GetConnectionString()));
    }

    private void ReconfigureDiscogs(IServiceCollection services)
    {
        services.RemoveAll<IClientConfig>();
        services.AddSingleton<IClientConfig>(_ => new ClientConfig
        {
            AuthToken = string.Empty,
            BaseUrl = _mockServer.GetPublicUrl()
        });

        services.RemoveAll<IDiscogsClient>();
        services.AddHttpClient<IDiscogsClient, DiscogsClient>(_ => _mockServer.CreateClient());
    }

    public HttpClient HttpClient { get; private set; } = default!;
    public IWireMockAdminApi WireMockApi { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await _mockServer.StartAsync();

        WireMockApi = _mockServer.CreateWireMockAdminClient();

        HttpClient = CreateClient();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _mockServer.StopAsync();
    }
}
