using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EspressoBudget.Api.Tests;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsOkWithHealthyStatus()
    {
        var ct = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/health", ct);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var health = await response.Content.ReadFromJsonAsync<HealthResponse>(ct);
        Assert.Equal("Healthy", health?.Status);
    }
}

public record HealthResponse(string Status);