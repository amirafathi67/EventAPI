using System.Text.Json;
using System.Text.Json.Serialization;
using EventAPI.Core.Interfaces;
using EventAPI.Core.Services;
using EventAPI;
using EventAPI.Auth;
using EventAPI.Core.Interfaces;
using EventAPI.Core.Services;
using EventAPI.Extensions;
using EventAPI.HealthCheck;
using EventAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using EventAPIe.Middleware;

var builder = WebApplication.CreateBuilder(args);
LoadAppSettingsConfigFile();
// Configure NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

var isDevEnv = builder.Environment.IsDevelopment();

builder.Services.AddMemoryCache();
builder.Services.AddControllers().AddJsonOptions(SetJsonOptions);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEventTicketMasterService, EventTicketMasterService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddHttpClient();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
}

app.UseHttpLoggerMiddleware();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
void LoadAppSettingsConfigFile()
{
    builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        
        .AddEnvironmentVariables();
}
void SetJsonOptions(JsonOptions options)
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
}
