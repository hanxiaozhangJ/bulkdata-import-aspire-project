// Program.cs - BulkDataImport.Migration
using System.Reflection;
using System.Linq;
using DbUp;
using Microsoft.Extensions.Configuration;

// Load configuration - Aspire will provide connection string via environment variables
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Get connection string from environment variable (set by Aspire)
// Aspire passes connection strings as ConnectionStrings__{ResourceName}
// Since we reference the "EntityBulkImport" database, the connection string will be:
// ConnectionStrings__EntityBulkImport
var connectionString = config.GetConnectionString("EntityBulkImport")
    ?? config.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__EntityBulkImport")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings:EntityBulkImport")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ No connection string found. Exiting.");
    Console.WriteLine("Expected environment variable: ConnectionStrings__DefaultConnection");
    Console.ResetColor();
    return;
}

Console.WriteLine("🔄 Running database migrations...");

// List embedded SQL scripts from assembly resources
var assembly = Assembly.GetExecutingAssembly();
var resourceNames = assembly.GetManifestResourceNames()
    .Where(name => name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
    .OrderBy(name => name)
    .ToList();

Console.WriteLine($"📋 Found {resourceNames.Count} migration script(s):");
foreach (var resourceName in resourceNames)
{
    var scriptName = resourceName.Contains('.') 
        ? resourceName.Substring(resourceName.LastIndexOf('.') + 1)
        : resourceName;
    Console.WriteLine($"   - {scriptName}");
}

// Ensure the DB exists
EnsureDatabase.For.PostgresqlDatabase(connectionString);

// Execute embedded SQL scripts
var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.EndsWith(".sql"))
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ Migration failed:");
    Console.WriteLine(result.Error);
    Console.ResetColor();
    return;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("✅ Migration completed successfully!");
Console.ResetColor();
