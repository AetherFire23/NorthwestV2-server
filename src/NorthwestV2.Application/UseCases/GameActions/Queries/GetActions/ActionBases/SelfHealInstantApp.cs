using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ActionBases.Bases;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ActionBases;

public class SelfHealInstantApp : InstantActionAppService
{
    private readonly SelfHealInstant _selfHealInstant;

    public SelfHealInstantApp(NorthwestContext context, SelfHealInstant selfHealInstant) : base(context)
    {
        _selfHealInstant = selfHealInstant;
    }

    public override async Task<InstantGameActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        var player = await Context.Players.FindById(request.PlayerId);

        InstantGameActionAvailability availability = _selfHealInstant.GetAvailability(player);

        return availability;
    }
}