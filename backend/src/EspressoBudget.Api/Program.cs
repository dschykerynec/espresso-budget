using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using EspressoBudget.Api.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Set ConnectionStrings:Database (user-secrets locally, or the ConnectionStrings__Database environment variable)");
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddNpgsqlDataSource(connectionString);

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database",
    tags: ["ready"], timeout: TimeSpan.FromSeconds(3));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = HealthResponseWriter.WriteJson
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready"),
    ResponseWriter = HealthResponseWriter.WriteJson
});

app.Run();
