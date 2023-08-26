using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.Application.Features.Plates;
using PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

[Collection("Shared collection")]
public class PlatesControllerTests
{
    private readonly WebApiFactory _factory;
    private readonly HttpClient _client;

    public PlatesControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.HttpClient;
    }

    [Fact]
    public async Task Create_ReturnsSuccessWithPlate()
    {
        // Arrange
        var command = new AddPlateCommand(11772);

        // Act
        var response = await _client.PostAsJsonAsync("api/plates", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var item = await response.Content.ReadFromJsonAsync<PlateDto>();

        item.Should().NotBeNull();

        var dbEntity = await GetPlateById(item!.Id);

        dbEntity.Should().BeEquivalentTo(item);
    }

    private async Task<Plate?> GetPlateById(Guid id)
    {
        var scope = _factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateRepository>();

        return await repository.GetPlateById(id);
    }
}
