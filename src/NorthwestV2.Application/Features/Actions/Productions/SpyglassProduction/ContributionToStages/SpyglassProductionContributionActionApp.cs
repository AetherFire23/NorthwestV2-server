using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.ContributionToStages;

public class SpyglassProductionContributionActionApp : InstantActionBase
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

    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        InstantActionAvailability? availability = _spyglassProductionContributionAction.DetermineAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        _spyglassProductionContributionAction.Contribute(player);
        
        await _unitOfWork.SaveChangesAsync();
    }
}