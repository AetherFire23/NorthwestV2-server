using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Initiation;

public class HammerProductionInitiationApp : InstantActionBase
{
    private readonly HammerProductionInitiation _hammerProductionInitiation;

    public HammerProductionInitiationApp(HammerProductionInitiation hammerProductionInitiation) : base(ActionNames
        .HammerProductionInitiation)
    {
        _hammerProductionInitiation = hammerProductionInitiation;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        InstantActionAvailability instantActionAvailability = _hammerProductionInitiation.DetermineAvailability();

        return instantActionAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}