using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NorthwestV2.Application.UseCases.Authentication.Login.Tokens;

public class JwtTokenService
{
    private readonly JwtConfig _config;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenService()
    {
        // TODO: use options instead one day 
        _config = new JwtConfig();
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> GenerateToken(Guid userId, string name)
    {
        List<Claim> claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, name)
        };

        // create claims for each role
        // so the USerRoles dont work
        // TODO: Configure different roles one day in the user like userRoles.Select(...)
        claims.Add(new Claim(ClaimTypes.Role, nameof(Roles.User)));

        // store security key
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpirationDays));

        var securityToken = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        string writenToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return await Task.FromResult(writenToken);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = _config.Issuer,
                //ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey)),
            };

            // Validate and parse the token
            var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

            // Ensure the token has the required claims or perform additional validations

            return principal;
        }
        catch (Exception)
        {
            // Token validation failed
            return null;
        }
    }

    private Claim CreateRoleClaim(Roles roleName)
    {
        return new Claim(ClaimTypes.Role, roleName.ToString());
    }
}