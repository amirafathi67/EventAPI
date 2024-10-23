namespace EventAPI.Auth;

public static class SecurityServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAuthentication(configuration);
        
    }

    public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        var useWildcards = configuration.GetSection("Cors:UseWildcards").Get<bool>();

        if (allowedOrigins != null && allowedOrigins.Any())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(optionsBuilder =>
                {
                    if (useWildcards)
                    {
                        optionsBuilder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    }

                    optionsBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition")
                        .WithOrigins(allowedOrigins);
                });
            });
        }

        return services;
    }
}