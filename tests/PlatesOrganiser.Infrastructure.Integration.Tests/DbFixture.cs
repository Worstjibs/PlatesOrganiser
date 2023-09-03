using Microsoft.EntityFrameworkCore;
using Npgsql;
using PlatesOrganiser.Infrastructure.Context;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace PlatesOrganiser.Infrastructure.Integration.Tests;

public class DbFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    public DbFixture()
    {
        _dbContainer = new PostgreSqlBuilder().Build();
    }

    public PlatesContext Context { get; set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<PlatesContext>().UseNpgsql(_dbContainer.GetConnectionString()).Options;
        Context = new PlatesContext(options);

        await Context.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            WithReseed = true
        });
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();

        await _dbConnection.CloseAsync();
        _dbConnection.Dispose();
    }

    public async Task ResetDb() => await _respawner.ResetAsync(_dbConnection);
}