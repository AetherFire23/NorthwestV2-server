using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Contribution;

public class HammerProductionContributionApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HammerProductionContribution _hammerProductionContribution;

    public HammerProductionContributionApp(IUnitOfWork unitOfWork,
        HammerProductionContribution hammerProductionContribution) : base(ActionNames.HammerProductionContribution)
    {
        _unitOfWork = unitOfWork;
        _hammerProductionContribution = hammerProductionContribution;
    }

    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        return new InstantActionAvailability()
        {
            ActionName = ActionNames.HammerProductionContribution,
            DisplayName = "Contribute points to hammer production. "
        };
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}