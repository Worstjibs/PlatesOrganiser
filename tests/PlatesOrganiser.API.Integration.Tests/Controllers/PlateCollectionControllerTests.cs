using PlatesOrganiser.API.Integration.Tests.Extensions;
using PlatesOrganiser.Application.Features.Collections;
using PlatesOrganiser.Application.Features.Collections.Commands.AddCollection;
using System.Net;
using System.Net.Http.Json;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

public class PlateCollectionControllerTests : IntegrationTestBase
{
    public PlateCollectionControllerTests(WebApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_GivenExistingCollection_ReturnsCollectionDto()
    {
        // Arrange
        var user = await ActAsUser(Guid.NewGuid());

        var collection = await _factory.AddCollectionAsync("Collection 1", user.Id);

        // Act
        var response = await _client.GetAsync($"api/collections/{collection.Id}");

        // Assert
        response.AssertOk();

        var dto = await response.Content.ReadFromJsonAsync<CollectionDto>();

        dto!.Id.Should().Be(collection.Id);
        dto.Name.Should().Be(collection.Name);
    }

    [Fact]
    public async Task Get_GivenNonExistingCollection_ReturnsNotFound()
    {
        // Arrange
        var user = await ActAsUser(Guid.NewGuid());

        // Act
        var response = await _client.GetAsync($"api/collections/{Guid.NewGuid()}");

        // Assert
        response.AssertNotFound();
    }

    [Fact]
    public async Task Add_CreatesANewCollection()
    {
        // Arrange
        var command = new AddCollectionCommand("Collection 1");

        await ActAsUser();

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var collectionId = await response.Content.ReadFromJsonAsync<Guid>();
        var collection = await _factory.GetCollectionById(collectionId);

        collection.Should().NotBeNull();
        collection.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Add_AddsCollectionToUser()
    {
        // Arrange
        var command = new AddCollectionCommand("Collection 123");

        var user = await ActAsUser();

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var collectionId = await response.Content.ReadFromJsonAsync<Guid>();

        var dbUser = await _factory.GetPlateUserById(user.Id);

        var userCollection = dbUser!.Collections.FirstOrDefault(x => x.Id == collectionId);
        userCollection.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Add_GivenNoCollectionName_ReturnsBadRequest(string? collectionName)
    {
        // Arrange
        var command = new AddCollectionCommand(collectionName!);

        await ActAsUser();

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Add_GivenDuplicateCollectionName_ReturnsBadRequest()
    {
        // Arrange
        var user = await ActAsUser(Guid.NewGuid());
        var collectionName = "Collection 123";

        await _factory.AddCollectionAsync(collectionName, user.Id);

        var command = new AddCollectionCommand(collectionName);

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
