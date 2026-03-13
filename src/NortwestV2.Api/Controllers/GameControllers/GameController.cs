using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Infrastructure;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers.GameControllers;

[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly NorthwestContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;
    private readonly NorthwestContext _context;

    public GameController(NorthwestContext ctx, ILogger<MyController> logger, IMediator mediator,
        NorthwestContext context)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
        _context = context;
    }

    [HttpGet("actions/availabilities")]
    public async Task<ActionResult<GetActionsResult>> GetActionsAvailabilities()
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

    [HttpPost("actions/execute")]
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