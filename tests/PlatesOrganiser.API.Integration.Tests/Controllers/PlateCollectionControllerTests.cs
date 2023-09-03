using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.API.Integration.Tests.Extensions;
using PlatesOrganiser.Application.Features.Collections.AddCollection;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;
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
        var collection = await _factory.GetCollectionById(collectionId);

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

        await ActAsUser(Guid.NewGuid());

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

        var collection = new PlateCollection { Name = collectionName, UserId = user.Id };
        await AddCollection(collection);

        var command = new AddCollectionCommand(collectionName);

        // Act
        var response = await _client.PostAsJsonAsync("api/collections", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task AddCollection(PlateCollection collection)
    {
        var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<PlatesContext>();

        context.Collections.Add(collection);

        await context.SaveChangesAsync();

        var dbCollection = await context.Collections.FirstOrDefaultAsync(x => x.Id == collection.Id);
    }
}
