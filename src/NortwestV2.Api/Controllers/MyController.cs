using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Infrastructure;
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


    // quand tu pick une game; ca met tes infos dans la session
    // dans le UI ce qu ca va faire :
    // setgame()
    // une fois fini, navigate dans le main() du site web
    // et puis pouf, toutes les request seront magiquement rendues aware du user. 


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

 
}

public class ExecuteBody()
{
    public string ActionName { get; set; }
    // public List
}