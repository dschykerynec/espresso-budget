using System.Net;
using System.Net.Http.Json;

namespace EspressoBudget.Api.Tests;

public class HealthEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LiveEndpoint_ReturnsOkWithNoChecks()
    {
        var ct = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/health/live", ct);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        HealthResponse? body = await response.Content.ReadFromJsonAsync<HealthResponse>(ct);
        Assert.NotNull(body);
        Assert.Empty(body.Checks);
        Assert.Equal("Healthy", body.Status);
    }

    [Fact]
    public async Task ReadyEndpoint_ReturnsOkWithDatabaseCheck()
    {
        var ct = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/health/ready", ct);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        HealthResponse? body = await response.Content.ReadFromJsonAsync<HealthResponse>(ct);
        Assert.NotNull(body);
        Assert.Equal("Healthy", body.Status);

        var dbCheck = Assert.Single(body.Checks);
        Assert.Equal("database", dbCheck.Name);
        Assert.Equal("Healthy", dbCheck.Status);
    }
}

public record HealthResponse(string Status, double TotalDurationMs, List<HealthCheckEntry> Checks);
public record HealthCheckEntry(string Name, string Status, string? Description, double DurationMs);