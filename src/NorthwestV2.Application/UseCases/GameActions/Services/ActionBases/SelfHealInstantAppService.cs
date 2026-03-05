using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases;

public class SelfHealInstantAppService : InstantActionAppService
{
    private readonly SelfHealInstant _selfHealInstant;

    public SelfHealInstantAppService(NorthwestContext context, SelfHealInstant selfHealInstant) : base(context)
    {
        _selfHealInstant = selfHealInstant;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await Context.Players.FindById(request.PlayerId);

        InstantActionAvailability availability = _selfHealInstant.GetAvailability(player);

        return availability;
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        return Task.CompletedTask;
    }
}