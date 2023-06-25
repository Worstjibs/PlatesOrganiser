namespace PlatesOrganiser.API.Integration.Tests;

[CollectionDefinition(CollectionName)]
public class SharedCollection : ICollectionFixture<WebApiFactory>
{
    public const string CollectionName = "Shared collection";
}