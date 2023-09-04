using System.Net;

namespace PlatesOrganiser.API.Integration.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static void AssertOk(this HttpResponseMessage response) =>
        response.StatusCode.Should().Be(HttpStatusCode.OK);

    public static void AssertCreated(this HttpResponseMessage response) =>
        response.StatusCode.Should().Be(HttpStatusCode.Created);

    public static void AssertBadRequest(this HttpResponseMessage response) =>
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    public static void AssertNotFound(this HttpResponseMessage response) =>
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
