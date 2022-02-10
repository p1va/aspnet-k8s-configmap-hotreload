using HotReload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

// Add appsettings.json config by default
var builder = WebApplication.CreateBuilder(args);

// Add environment variables as configuration
builder.Configuration.AddEnvironmentVariables();

// Add other JSON files on top of appsettings.json. Changes here are going to override appsettings one
builder.Configuration.AddJsonFile(path: "config/providers/providers.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile(path: "config/feature/featureManagement.json", optional: true, reloadOnChange: true);

// Add options to the DI
builder.Services.Configure<ProviderServiceOptions>(builder.Configuration.GetSection("ProviderService"));

// Enable feature management
// By default this is going to bind to "FeatureManagement" config key
builder.Services.AddFeatureManagement();

// Add services
builder.Services.AddScoped<IProviderService, OptionsSnapshotProviderService>();
builder.Services.AddScoped<IAuthUriService, AuthUriService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Expose the providers endpoint
app.MapGet("/providers", ([FromServices] IProviderService providers) => new
{
    results = providers.Get()
});

// Expose the Auth URI endpoint
app.MapGet("/authuri", async ([FromServices] IAuthUriService authUri) => new
{
    auth_auth = await authUri.GetAsync()
});

app.Run();