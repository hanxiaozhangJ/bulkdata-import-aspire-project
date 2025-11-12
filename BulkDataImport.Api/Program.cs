using Serilog;
using Serilog.Filters;
using BulkDataImport.Api;
using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;

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

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

// Enable static files (for validation test page)
app.UseStaticFiles();

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

// Validation endpoint to demonstrate FluentValidation with ValidationProblem
app.MapPost("/validation", async (IValidator<ValidationRequest> validator, ValidationRequest request) =>
{
    var validationResult = await validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
        // Convert FluentValidation errors to dictionary format for ValidationProblem
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
        
        return Results.ValidationProblem(errors);
    }

    return Results.Ok(new { message = "Validation successful", request });
})
    .WithName("validation")
    .AllowAnonymous()
    .Accepts<ValidationRequest>("application/json")
    .Produces<object>(StatusCodes.Status200OK)
    .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest);

app.Run();
