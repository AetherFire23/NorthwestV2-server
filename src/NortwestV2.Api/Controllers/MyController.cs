using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
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

    [HttpPut("getitems")]
    public async Task<ActionResult> GetPlayers()
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        var items = _context.Players
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == userData.PlayerId).Inventory.Items.ToList();

        return Ok();
    }

    [HttpGet("availabilities")]
    public async Task<ActionResult> GetActionsAvailabilities()
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        if (!userData.PlayerId.HasValue)
        {
            throw new Exception("This data is required");
        }

        GetActionsResult result = await _mediator.Send(new GetActionsRequest()
        {
            PlayerId = userData.PlayerId.Value
        });

        return Ok(result);
    }

    [HttpPost("execute")]
    public async Task<ActionResult> Execute()
    {
        UserData userData = this.HttpContext.Session.GetUserData();
        //
        // await _mediator.Send(new ExecuteActionRequest()
        // {
        //     PlayerId = userData.PlayerId,
        //     ActionName = actionName,
        //     ActionTargets = 
        // })
        return Ok();
    }
}

public class ExecuteBody()
{
    public string ActionName { get; set; }
    // public List
}