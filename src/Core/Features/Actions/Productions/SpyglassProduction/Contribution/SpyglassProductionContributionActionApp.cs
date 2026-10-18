using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.Productions.SpyglassProduction.Contribution;

public class SpyglassProductionContributionActionApp : ActionWithTargetsBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly SpyglassProductionContributionAction _spyglassProductionContributionAction;

    public SpyglassProductionContributionActionApp(
        SpyglassProductionContributionAction spyglassProductionContributionAction,
        IPlayerRepository playerRepository, IUnitOfWork unitOfWork) : base(ActionNames
        .SpyglassContribution)
    {
        _spyglassProductionContributionAction = spyglassProductionContributionAction;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        ActionWithTargetsAvailability? availability = _spyglassProductionContributionAction.DetermineAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        _spyglassProductionContributionAction.Contribute(player, ActionTargetsList.From(request.ActionTargets));

        await _unitOfWork.SaveChangesAsync();
    }
}
