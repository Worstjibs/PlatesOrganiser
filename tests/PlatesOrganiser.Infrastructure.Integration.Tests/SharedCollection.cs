namespace PlatesOrganiser.Infrastructure.Integration.Tests;

[CollectionDefinition(CollectionName)]
public class SharedCollection : ICollectionFixture<DbFixture>
{
    public const string CollectionName = "Shared collection";
}
