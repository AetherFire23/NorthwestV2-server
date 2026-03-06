using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("auth")]
public class MyController : ControllerBase
{
    private readonly NorthwestContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;
    private readonly NorthwestContext _context;

    public MyController(NorthwestContext ctx, ILogger<MyController> logger, IMediator mediator,
        NorthwestContext context)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register(RegisterRequest request,
        CancellationToken cancellationToken)
    {
        Guid userId = await _mediator.Send(request, cancellationToken);
        return Ok(userId);
    }

    [HttpPut("login")]
    public async Task<ActionResult<Guid>> Login(LoginRequest request,
        CancellationToken cancellationToken)
    {
        LoginResult lg = await _mediator.Send(request, cancellationToken);

        this.HttpContext.Session.SetUserData(new UserData
        {
            UserId = lg.UserId
        });

        return Ok(lg);
    }

    [HttpGet("games")]
    public async Task<ActionResult> GetGames()
    {
        return Ok();
    }

    // quand tu pick une game; ca met tes infos dans la session
    // dans le UI ce qu ca va faire :
    // setgame()
    // une fois fini, navigate dans le main() du site web
    // et puis pouf, toutes les request seront magiquement rendues aware du user. 


    /// <summary>
    /// Is purely a session request; does not provide any new info. 
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    [HttpPut("setgame")]
    public async Task<ActionResult> Test(Guid gameId)
    {
        Game game = _context.Games
            .Include(x => x.Players)
            .First(x => x.Id == gameId);

        UserData userData = this.HttpContext.Session.GetUserData();

        Player player = game.Players.First(x => x.UserId == userData.UserId);

        userData.PlayerId = player.Id;

        userData.GameId = gameId;

        HttpContext.Session.SetUserData(userData);

        return Ok();
    }
}