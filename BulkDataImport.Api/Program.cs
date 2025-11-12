using Serilog;
using Serilog.Filters;
using BulkDataImport.Api;
using Microsoft.AspNetCore.Diagnostics;

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

// Add ProblemDetails service for standardized error responses
builder.Services.AddProblemDetails();

// Register the custom exception handler
// The UseExceptionHandler middleware will automatically discover IExceptionHandler services
builder.Services.AddSingleton<IExceptionHandler, DefaultExceptionHandler>();

var app = builder.Build();

// Add exception handling middleware (must be early in the pipeline)
app.UseExceptionHandler();

// Add status code pages middleware for handling non-exception errors (404, etc.)
app.UseStatusCodePages();

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

// Exception endpoint for testing error handling
app.MapGet("/exception", () => { throw new Exception("Test exception for error handling"); })
    .WithName("exception")
    .AllowAnonymous();

app.Run();
