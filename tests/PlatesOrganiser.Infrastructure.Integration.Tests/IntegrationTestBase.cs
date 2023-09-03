namespace PlatesOrganiser.Infrastructure.Integration.Tests;

[Collection(SharedCollection.CollectionName)]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly DbFixture _fixture;

    protected IntegrationTestBase(DbFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task DisposeAsync() => await _fixture.ResetDb();

    public Task InitializeAsync() => Task.CompletedTask;
}
