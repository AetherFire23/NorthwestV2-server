using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.Productions.FishingPole.Contribution;

public class FishingPoleContributionApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly FishingPoleContribution _fishingPoleContribution;

    public FishingPoleContributionApp(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        FishingPoleContribution fishingPoleContribution) :
        base(ActionNames.ContributeFishingPoleProduction)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _fishingPoleContribution = fishingPoleContribution;
    }


    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        InstantActionAvailability availability = _fishingPoleContribution.DetermineAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        // TODO: Fishing pole contribution 
        throw new NotImplementedException();

        await _unitOfWork.SaveChangesAsync();
    }
}