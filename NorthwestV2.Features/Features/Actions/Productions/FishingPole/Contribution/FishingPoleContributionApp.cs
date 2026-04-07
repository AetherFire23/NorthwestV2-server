using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;

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
        throw new NotImplementedException();

        await _unitOfWork.SaveChangesAsync();
    }
}