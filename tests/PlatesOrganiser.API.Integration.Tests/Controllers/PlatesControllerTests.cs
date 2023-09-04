using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Namotion.Reflection;
using ParkSquare.Discogs.Dto;
using PlatesOrganiser.API.Integration.Tests.Auth;
using PlatesOrganiser.API.Integration.Tests.Extensions;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Enum;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Fakes;
using System.Net;
using System.Net.Http.Json;
using WireMock.Client.Extensions;
using Discogs = ParkSquare.Discogs.Dto;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

public class PlatesControllerTests : IntegrationTestBase
{
    public PlatesControllerTests(WebApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_ReturnsCreatedAndCreatesPlate()
    {
        // Arrange
        var command = new AddPlateCommand(_random.Next(10000));

        var masterRelease = await SetupMasterReleaseRequest(command.MasterReleaseId);
        var versions = await SetupMasterReleaseVersionsRequest(10, command.MasterReleaseId);

        await ActAsUser();

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.AssertCreated();

        var plateId = await response.Content.ReadFromJsonAsync<Guid>();

        var dbEntity = await _factory.GetPlateById(plateId);

        dbEntity!.Name.Should().BeEquivalentTo(masterRelease.Title);
        dbEntity.DiscogsMasterReleaseId.Should().Be(masterRelease.MasterId);
        dbEntity.PrimaryLabel.Name.Should().Be(versions.First().Label);
    }

    [Fact]
    public async Task Create_GivenNonExistentUser_CreatesUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddPlateCommand(_random.Next(10000));

        await SetupMasterReleaseRequest(command.MasterReleaseId);
        await SetupMasterReleaseVersionsRequest(10, command.MasterReleaseId);

        _client.ActAsUser(userId);

        (await _factory.GetPlateUserById(userId)).Should().BeNull();

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.AssertCreated();

        (await _factory.GetPlateUserById(userId)).Should().NotBeNull();
    }

    [Fact]
    public async Task Create_AddsToTheUsersCollection()
    {
        // Arrange
        var command = new AddPlateCommand(_random.Next(10000));

        await SetupMasterReleaseRequest(command.MasterReleaseId);
        await SetupMasterReleaseVersionsRequest(10, command.MasterReleaseId);

        var user = await ActAsUser();

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.AssertCreated();

        var plateId = await response.Content.ReadFromJsonAsync<Guid>();

        var dbPlate = await _factory.GetPlateById(plateId);

        var dbUser = await _factory.GetPlateUserById(user.Id);
        var defaultCollection = dbUser!.Collections.First(x => x.Type == CollectionType.Default);

        var collectionPlate = defaultCollection.Plates.First(x => x.Id == plateId);

        collectionPlate.Should().BeEquivalentTo(dbPlate, config => config.Excluding(x => x.PrimaryLabel));
    }

    private async Task<MasterRelease> SetupMasterReleaseRequest(int masterReleaseId)
    {
        var builder = _wireMock.GetMappingBuilder();

        var masterRelease = Fake.MasterRelease(masterReleaseId);

        builder.Given(m => m
                   .WithRequest(req => req
                        .UsingGet()
                        .WithPath($"/masters/{masterReleaseId}"))
                   .WithResponse(rsp => rsp
                        .WithBodyAsJson(masterRelease)));

        await builder.BuildAndPostAsync();

        return masterRelease;
    }

    private async Task<IEnumerable<Discogs.Version>> SetupMasterReleaseVersionsRequest(int count, int masterReleaseId)
    {
        var versions = Fake.Versions(count).ToList();
        var versionResults = new VersionResults
        {
            Pagination = new Pagination
            {
                Pages = 1
            },
            Versions = versions
        };

        var builder = _wireMock.GetMappingBuilder();

        builder.Given(m => m
                   .WithRequest(req => req
                        .UsingGet()
                        .WithPath($"/masters/{masterReleaseId}/versions"))
                   .WithResponse(rsp => rsp
                        .WithBodyAsJson(versionResults)));

        await builder.BuildAndPostAsync();

        return versions;
    }
}
