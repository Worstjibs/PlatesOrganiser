using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using ParkSquare.Discogs.Dto;
using PlatesOrganiser.API.Integration.Tests.Auth;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Fakes;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using WireMock.Client.Extensions;
using Discogs = ParkSquare.Discogs.Dto;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

public class PlatesControllerTests : IntegrationTestBase
{
    public PlatesControllerTests(WebApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_ReturnsSuccessWithPlate()
    {
        // Arrange
        var command = new AddPlateCommand(_random.Next(10000));

        var masterRelease = await SetupMasterReleaseRequest(command.MasterReleaseId);
        await SetupMasterReleaseVersionsRequest(10, command.MasterReleaseId);

        _client.ActAsUser(Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var item = await response.Content.ReadFromJsonAsync<PlateDto>();

        item.Should().NotBeNull();
        item!.Name.Should().Be(masterRelease.Title);
        item.DiscogsMasterReleaseId.Should().Be(command.MasterReleaseId);

        var dbEntity = await GetPlateById(item!.Id);

        dbEntity.Should().BeEquivalentTo(item);
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

    private async Task<Plate?> GetPlateById(Guid id)
    {
        var scope = _factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateRepository>();

        return await repository.GetPlateById(id);
    }
}
