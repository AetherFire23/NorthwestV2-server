using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution;

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