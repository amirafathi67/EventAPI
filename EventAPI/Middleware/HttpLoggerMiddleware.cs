namespace EventAPI.Middleware;

public class HttpLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpLoggerMiddleware> _logger;

    public HttpLoggerMiddleware(RequestDelegate next, ILogger<HttpLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestUrl = context.Request.Path;
        var requestMethod = context.Request.Method;

        await _next.Invoke(context);

        var responseCode = context.Response.StatusCode;

        if (context.Response.StatusCode < 200 ||
            context.Response.StatusCode >= 300)
            _logger.LogDebug($"Request: {requestMethod} {requestUrl}" +
                             $"{Environment.NewLine}      Response: {responseCode}");
    }
}

public static class HttpLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpLoggerMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpLoggerMiddleware>();
    }
}