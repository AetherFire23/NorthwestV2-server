using System.ComponentModel.DataAnnotations;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Features.UseCases.GameLogs.Queries;
using NorthwestV2.Features.UseCases.Items.Commands;
using NorthwestV2.Features.UseCases.Items.Queries;
using NorthwestV2.Features.UseCases.OtherPlayers.Queries;
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
            throw new Exception("A player id needs to be set first before calling this endpoint. ");
        }

        GetActionsResult result = await _mediator.Send(new GetActionsRequest()
        {
            PlayerId = userData.PlayerId.Value
        });

        return Ok(result);
    }

    [HttpPost("execute")]
    public async Task<ActionResult<string>> Execute(ExecuteActionApiRequest apiRequest)
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        await _mediator.Send(new ExecuteActionRequest()
        {
            ActionName = apiRequest.ActionName,
            PlayerId = userData.PlayerId.Value,
            ActionTargets = apiRequest.Targets,
        });

        return Ok("executed successfully");
    }


    // TODO: Change response type to something real. 
    [HttpGet("othersInRoom")]
    public async Task<ActionResult<GetOtherPlayersResponse>> GetPlayersInRoom()
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        GetOtherPlayersResponse getOtherPlayersResponse = await _mediator.Send(new GetOtherPlayersRequest()
        {
            PlayerId = userData.PlayerId.Value
        });

        return Ok(getOtherPlayersResponse);
    }

    // TODO: Change response type to something real. 
    [HttpGet("available-items")]
    public async Task<ActionResult<GetAvailableItemsResponse>> GetAvailableItems()
    {
        UserData userData = this.HttpContext.Session.GetUserData();

        GetAvailableItemsResponse availableItemsResponse = await _mediator.Send(new GetAvailableItemsRequest
        {
            PlayerId = userData.PlayerId!.Value
        });

        return Ok(availableItemsResponse);
    }

    /// <summary>
    /// Swaps an item from room to player or from player to room.
    /// Throws an exception if the item is not in either of those. 
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [HttpPost("swap-item-in-room")]
    public async Task<ActionResult> SwapItemsInRoom([FromQuery] Guid itemId)
    {
        UserData userData = HttpContext.Session.GetUserData();

        await _mediator.Send(new SwapItemOwnershipRequest
        {
            PlayerId = userData.PlayerId.Value,
            ItemId = itemId
        });

        return Ok();
    }

    [HttpGet("get-available-logs")]
    public async Task<ActionResult<GetGameLogsResponse>> GetLogs()
    {
        UserData userData = HttpContext.Session.GetUserData();

        GetGameLogsResponse logs = await _mediator.Send(new GetGameLogsRequest()
        {
            PlayerId = userData.PlayerId.Value,
        });

        return Ok(logs);
    }

    // TODO: Endpoint for Logs 
}

public class ExecuteActionApiRequest
{
    public string ActionName { get; set; }
    public List<List<ActionTarget>> Targets { get; set; }
}