using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases;

public class DebugTargetsActionApp : ActionWithTargetsAppService
{
    private readonly DebugTargetAction _debugTargetAction;

    public DebugTargetsActionApp(NorthwestContext context, DebugTargetAction debugTargetAction) : base(context, " ")
    {
        _debugTargetAction = debugTargetAction;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);
        List<Player> allPlayersIngame =
            Context.Players.Where(x => x.GameId == player.GameId && x.Id != request.PlayerId).ToList();
        ActionWithTargetsAvailability availability = _debugTargetAction.GetAvailability(player, allPlayersIngame);

        return availability;
    }
}