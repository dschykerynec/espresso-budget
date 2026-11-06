using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EspressoBudget.Api.HealthChecks;

public static class HealthResponseWriter
{
    public static Task WriteJson(HttpContext context, HealthReport report)
    {
        var responseBody = new
        {
            status = report.Status.ToString(),
            totalDurationMs = Math.Round(report.TotalDuration.TotalMilliseconds),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                durationMs = Math.Round(e.Value.Duration.TotalMilliseconds)
            })
        };
        return context.Response.WriteAsJsonAsync(responseBody);
    }
}