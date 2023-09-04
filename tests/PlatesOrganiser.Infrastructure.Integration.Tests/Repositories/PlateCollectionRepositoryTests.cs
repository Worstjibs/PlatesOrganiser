using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Fakes;
using PlatesOrganiser.Infrastructure.Repositories;
using Xunit;

namespace PlatesOrganiser.Infrastructure.Integration.Tests.Repositories;

public class PlateCollectionRepositoryTests : IntegrationTestBase
{
    private readonly IPlateCollectionRepository _repository;

    public PlateCollectionRepositoryTests(DbFixture fixture) : base(fixture)
    {
        _repository = new PlateCollectionRepository(fixture.Context);
    }

    [Fact]
    public async Task AddCollection_CreatesACollection()
    {
        // Arrange
        var user = new PlateUser { UserName = "TestUser" };

        _fixture.Context.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var collection = new PlateCollection { Name = "Collection 1", User = user };

        // Act
        _repository.AddCollection(collection, CancellationToken.None);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var dbEntity = await _fixture.Context.Collections.FirstOrDefaultAsync(x => x.Id == collection.Id);

        dbEntity.Should().NotBeNull();
        dbEntity.Should().BeEquivalentTo(collection);
    }

    [Fact]
    public async Task GetCollectionByIdAsync_GivenExistingCollection_ReturnsACollection()
    {
        // Arrange
        var user = new PlateUser { UserName = "TestUser" };
        var collection = new PlateCollection { Name = "Collection 1", User = user };

        _fixture.Context.Add(user);
        _fixture.Context.Add(collection);

        await _fixture.Context.SaveChangesAsync();

        // Act
        var dbEntity = await _repository.GetCollectionByIdAsync(collection.Id, CancellationToken.None);

        // Assert
        dbEntity.Should().BeEquivalentTo(collection);
    }

    [Fact]
    public async Task CollectionExistsAsync_GivenNoDuplicateCollectionNames_ReturnsFalse()
    {
        // Arrange
        var user = new PlateUser { UserName = "TestUser" };

        _fixture.Context.Add(user);

        await _fixture.Context.SaveChangesAsync();

        // Act
        var result = await _repository.CollectionExistsAsync("Collection 1", user.Id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CollectionExistsAsync_GivenDuplicateCollectionNameForUser_ReturnsTrue()
    {
        // Arrange
        var collectionName = "Collection 1";

        var user = new PlateUser { UserName = "TestUser" };
        var collection = new PlateCollection { Name = collectionName, User = user };

        _fixture.Context.Add(user);
        _fixture.Context.Add(collection);

        await _fixture.Context.SaveChangesAsync();

        // Act
        var result = await _repository.CollectionExistsAsync(collectionName, user.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CollectionExistsAsync_GivenDuplicateCollectionNameForDifferentUser_ReturnsFalse()
    {
        // Arrange
        var collectionName = "Collection 1";

        var user = new PlateUser { UserName = "TestUser" };
        var user2 = new PlateUser { UserName = "TestUser2" };
        var collection = new PlateCollection { Name = collectionName, User = user2 };

        _fixture.Context.AddRange(new[] { user, user2 });
        _fixture.Context.Add(collection);

        await _fixture.Context.SaveChangesAsync();

        // Act
        var result = await _repository.CollectionExistsAsync(collectionName, user.Id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetCollectionsAsync_ReturnsPagedListOfCollections()
    {
        // Arrange
        var user = Fake.PlateUser();

        // Act


        // Assert

    }
}
