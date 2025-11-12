using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog logging
builder.Logging.ClearProviders();
builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Services(services)
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
#if DEBUG
        .WriteTo.Console()
#endif
        .Filter.ByExcluding(Matching.FromSource<Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager>())
        .Filter.ByExcluding(Matching.FromSource<Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository>());
});

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Add request logging middleware for debugging
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Simple ping endpoint - accessible via both HTTP and HTTPS
app.MapGet("/ping", (ILogger<Program> logger) =>
{
    logger.LogInformation("Ping endpoint called");
    return Results.Ok("pong");
})
    .WithName("Ping")
    .AllowAnonymous();

// Root endpoint for testing
app.MapGet("/", () => Results.Ok("API is running. Try /ping endpoint."))
    .WithName("Root")
    .AllowAnonymous();

app.Run();
