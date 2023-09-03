using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.Application.Features.Collections.AddCollection;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

public class PlateCollectionControllerTests : IntegrationTestBase
{
    public PlateCollectionControllerTests(WebApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Add_CreatesANewCollection()
    {
        // Arrange
        var command = new AddCollectionCommand("Collection 1");

        await ActAsUser(Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var collectionId = await response.Content.ReadFromJsonAsync<Guid>();
        var collection = await GetCollectionById(collectionId);

        collection.Should().NotBeNull();
        collection.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Add_AddsCollectionToUser()
    {
        // Arrange
        var command = new AddCollectionCommand("Collection 123");

        var user = await ActAsUser(Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var collectionId = await response.Content.ReadFromJsonAsync<Guid>();

        var dbUser = await GetUserById(user.Id);

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

        await ActAsUser(Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<PlateCollection?> GetCollectionById(Guid id)
    {
        var scope = _factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateCollectionRepository>();

        return await repository.GetCollectionByIdAsync(id, CancellationToken.None);
    }

    private async Task<PlateUser?> GetUserById(Guid id)
    {
        var scope = _factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateUserRepository>();

        return await repository.GetUserByIdAsync(id);
    }
}
