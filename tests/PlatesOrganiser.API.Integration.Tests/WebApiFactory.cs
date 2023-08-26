using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlatesOrganiser.Infrastructure.Context;
using Testcontainers.PostgreSql;

namespace PlatesOrganiser.API.Integration.Tests;

public class WebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(config =>
        {
            config.RemoveAll<PlatesContext>();
            config.RemoveAll<DbContextOptions<PlatesContext>>();

            config.AddDbContext<PlatesContext>(config => config.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public HttpClient HttpClient { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        HttpClient = CreateClient();
    }

    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
