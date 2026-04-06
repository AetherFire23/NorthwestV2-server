using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Infrastructure;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("mainmenu")]
public class MainMenuController : ControllerBase
{
    private readonly NorthwestContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;
    private readonly NorthwestContext _context;

    public MainMenuController(NorthwestContext ctx, ILogger<MyController> logger, IMediator mediator,
        NorthwestContext context)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Is purely a session request; does not provide any new info. 
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    [HttpPut("joingame")]
    public async Task<ActionResult> Test()
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        Player player = _context.Players.First(x => x.UserId == userData.UserId);

        userData.PlayerId = player.Id;

        userData.GameId = player.GameId;

        HttpContext.Session.SetUserData(userData);

        return Ok();
    }
}