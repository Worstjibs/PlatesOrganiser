using Microsoft.Extensions.DependencyInjection;
using ParkSquare.Discogs.Dto;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using WireMock.Client;
using WireMock.Client.Extensions;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

[Collection("Shared collection")]
public class PlatesControllerTests
{
    private readonly Random _random = new Random(808);

    private readonly WebApiFactory _factory;
    private readonly IWireMockAdminApi _wireMock;
    private readonly HttpClient _client;

    public PlatesControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _wireMock = factory.WireMockApi;
        _client = factory.HttpClient;
    }

    [Fact]
    public async Task Create_ReturnsSuccessWithPlate()
    {
        // Arrange
        var command = new AddPlateCommand(_random.Next(10000));

        await SetupDiscogs(command.MasterReleaseId);

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var item = await response.Content.ReadFromJsonAsync<PlateDto>();

        item.Should().NotBeNull();

        var dbEntity = await GetPlateById(item!.Id);

        dbEntity.Should().BeEquivalentTo(item);
    }

    private async Task SetupDiscogs(int masterReleaseId)
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
    }

    private async Task<Plate?> GetPlateById(Guid id)
    {
        var scope = _factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateRepository>();

        return await repository.GetPlateById(id);
    }
}
