using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.ByRoles.Debug;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.Instant;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases;

public class SelfHealInstantApp : InstantActionBase
{
    private readonly DebugInstantAction _debugInstantAction;

    public SelfHealInstantApp(NorthwestContext context, DebugInstantAction debugInstantAction) : base(context,
        ActionNames.InstantHeal)
    {
        _debugInstantAction = debugInstantAction;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);

        InstantActionAvailability availability = _debugInstantAction.GetAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);

        player.Health += 2;

        await Context.SaveChangesAsync();
    }
}