using System.Security.Claims;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Login.Tokens;

namespace ERP.Testing.Domain.UseCases.Authentication.Login.Tokens;

[TestSubject(typeof(JwtTokenService))]
public class JwtTokenServiceTest
{
    [Fact]
    public void GivenJwTokenServce_canIssue_token()
    {
        var jwtManager = new JwtTokenService();


        string token = jwtManager.GenerateToken(Guid.NewGuid());
        
        Assert.True(token != null);
    }
    
    [Fact]
    public void GivenJwTokenServce_canValidateToken_Corecctly()
    {
        var jwtManager = new JwtTokenService();

        string token = jwtManager.GenerateToken(Guid.NewGuid());

        var isValid = jwtManager.ValidateToken(token);

        Assert.NotNull(isValid);
    }
}