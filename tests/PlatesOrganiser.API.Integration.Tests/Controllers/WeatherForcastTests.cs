using System.Net;
using System.Net.Http.Json;

namespace PlatesOrganiser.API.Integration.Tests.Controllers;

[Collection(SharedCollection.CollectionName)]
public class WeatherForcastTests
{
    private readonly WebApiFactory _apiFactory;
    private readonly HttpClient _client;

    private const string BaseEndpoint = "weatherforecast";

    public WeatherForcastTests(WebApiFactory apiFactory)
    {
        _apiFactory = apiFactory;

        _client = apiFactory.HttpClient;
    }

    [Fact]
    public async Task Get_ReturnsListOfForecasts()
    {
        // Arrange


        // Act
        var response = await _client.GetAsync(BaseEndpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        results.Should().NotBeEmpty();
    }
}
