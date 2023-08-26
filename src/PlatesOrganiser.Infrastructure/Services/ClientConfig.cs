using ParkSquare.Discogs;

namespace PlatesOrganiser.Infrastructure.Services;

public class ClientConfig : IClientConfig
{
    public string AuthToken { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
