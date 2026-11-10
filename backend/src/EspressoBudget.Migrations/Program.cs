using System.Reflection;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();
var connectionString = config.GetConnectionString("Database");


if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Connection string is not set. Please set the 'ConnectionStrings__Database' environment variable.");
    return 1;
}

UpgradeEngine upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

    var result = upgrader.PerformUpgrade();
    
    if (!result.Successful)
    {
        Console.Error.WriteLine("Error during database migration. See error above.");
        return 1;
    }

    Console.WriteLine("Database migration completed successfully.");
    return 0;