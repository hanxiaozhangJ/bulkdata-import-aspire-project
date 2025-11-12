var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Simple ping endpoint
app.MapGet("/ping", () => "pong")
    .WithName("Ping")
    .WithOpenApi();

app.Run();
