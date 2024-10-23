using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EventAPI.Extensions;

public class TokenGenerator(IConfiguration configuration)
{
    public string CreateToken(int expiryMinutes = 30)
    {

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:SecretKey"]!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddMinutes(expiryMinutes);
        var claims = new[]
       {
            new Claim("urn:Event:userauthenticationtype", "Event"),
            new Claim("urn:Event:expireUtc", expiry.Ticks.ToString()),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(

            issuer: configuration["Authentication:Issuer"],
            audience: configuration["Authentication:Issuer"],
            claims,
            expires: expiry,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}