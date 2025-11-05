using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.SignalR;
using calculator.web.Data;
using calculator.web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// Add health checks
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    // Detailed errors for development
    options.DetailedErrors = builder.Environment.IsDevelopment();

    // Configure circuit options
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.DisconnectedCircuitMaxRetained = 100;
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
});

// Configure SignalR hub options
builder.Services.Configure<HubOptions>(options =>
{
    // Maximum message size (32 KB default, increase if needed)
    options.MaximumReceiveMessageSize = 32 * 1024;

    // Client timeout interval (30 seconds)
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);

    // Keep alive interval (15 seconds)
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});

// Register scoped services (scoped to the Blazor circuit/user session)
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<ICalculationHistoryService, CalculationHistoryService>();

// Register custom circuit handler for connection monitoring
builder.Services.AddScoped<CircuitHandler, CalculatorCircuitHandler>();

// Keep the weather forecast service for template pages
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
