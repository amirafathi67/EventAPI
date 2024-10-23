using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventAPI.Auth;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthentication(this IServiceCollection collection,
        IConfiguration configuration)
    {
        var issuer = configuration.GetValue<string>("Authentication:Issuer");
        var secretKey = configuration.GetValue<string>("Authentication:SecretKey");
        collection
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return collection;
    }
}