using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;

namespace EventAPI.Extensions;

public static class SwaggerConfiguration
{
    private const string ApiDescriptionMarkDown = @"
## Authentication

All requests to this Api must have the *Authorization* header containing the bearer token from Gx.

Generated JWT:
```http
Bearer {0}
```
";

    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenGenerator = new TokenGenerator(configuration);
      
        services.AddSwaggerDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
                document.Info.Title = "Event Service API";
                document.Info.Description = ApiDescriptionMarkDown.Replace("{0}", tokenGenerator.CreateToken());
            };

            const string securitySchemeName = "Bearer Token";
            config.AddSecurity(securitySchemeName, Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header
            });

            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(securitySchemeName));
        });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi
            (c => { c.Path = string.Empty; });
        //app.UseSwaggerUI(config =>
        //{
        //    config.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1");

        //});
    }
}