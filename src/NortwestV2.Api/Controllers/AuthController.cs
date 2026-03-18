using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Infrastructure;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly NorthwestContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;
    private readonly NorthwestContext _context;

    public AuthController(NorthwestContext ctx, ILogger<MyController> logger, IMediator mediator,
        NorthwestContext context)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Allo allo am i documented ?
    /// </summary>
    /// <param name="request"></param>
    /// <returns>User id </returns>
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register(RegisterRequest request,
        CancellationToken cancellationToken)
    {
        Guid userId = await _mediator.Send(request, cancellationToken);
        return Ok(userId);
    }

    [HttpPut("login")]
    [ProducesResponseType<LoginResult>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Login(LoginRequest request,
        CancellationToken cancellationToken)
    {
        LoginResult lg = await _mediator.Send(request, cancellationToken);
        
        // TODO: set the default player I g
        this.Response.Cookies.Append("nw_token", lg.Token, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(420)
        });
        
        Player playerId = this._context.Players.First(x => x.UserId == lg.UserId);
        this.HttpContext.Session.SetUserData(new UserData
        {
            UserId = lg.UserId,
            PlayerId = playerId.Id,
            GameId = playerId.GameId
        });


        return Ok(lg);
    }
}