using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EventAPI.Extensions;

public static class HealthCheck
{
    public static Task WriteResponse(
        HttpContext context,
        HealthReport report)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        string json = JsonSerializer.Serialize(
            new
            {
                Status = report.Status.ToString(),
                Info = report.Entries
                    .Select(e =>
                        new
                        {
                            e.Key,
                            e.Value.Description,
                            Status = Enum.GetName(
                                typeof(HealthStatus),
                                e.Value.Status),
                            Error = e.Value.Exception?.Message
                        })
                    .ToList()
            },
            jsonSerializerOptions);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(json);
    }
}