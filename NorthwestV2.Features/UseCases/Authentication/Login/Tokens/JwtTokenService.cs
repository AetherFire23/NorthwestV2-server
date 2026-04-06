using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NorthwestV2.Features.UseCases.Authentication.Login.Tokens;

public class JwtTokenService
{
    private readonly JwtConfig _config;
    private readonly byte[] _key;

    public JwtTokenService()
    {
        _config = new JwtConfig();
        _key = Encoding.UTF8.GetBytes(_config.SecretKey);
    }

    public string GenerateToken(Guid userId)
    {
        SigningCredentials credentials = new SigningCredentials(
            new SymmetricSecurityKey(_key),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_config.ExpirationDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        try
        {
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(_key)
            }, out _);
        }
        catch
        {
            return null;
        }
    }
}