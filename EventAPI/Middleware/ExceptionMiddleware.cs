using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventAPIe.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var errorId = Guid.NewGuid();
        _logger.LogError(ex, "Error Id: {errorId}", errorId);

        if (ex is HttpRequestException)
        {
            var re = ex as HttpRequestException;
            context.Response.StatusCode = (int)re.StatusCode.Value;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        context.Response.ContentType = "application/json";

        var result = new
        {
            message = $"An unexpected error has occurred. Please contact your system administrator. Error Id: {errorId}"
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(result,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            )
        );
    }
}