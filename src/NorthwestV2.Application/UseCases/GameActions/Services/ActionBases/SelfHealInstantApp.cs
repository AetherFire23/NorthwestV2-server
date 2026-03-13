using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases;

public class SelfHealInstantApp : InstantActionBase
{
    private readonly SelfHealInstantAction _selfHealInstantAction;

    public SelfHealInstantApp(NorthwestContext context, SelfHealInstantAction selfHealInstantAction) : base(context,
        ActionNames.InstantHeal)
    {
        _selfHealInstantAction = selfHealInstantAction;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);

        InstantActionAvailability availability = _selfHealInstantAction.GetAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);
        
        _selfHealInstantAction.Execute(player);

        await Context.SaveChangesAsync();
    }
}