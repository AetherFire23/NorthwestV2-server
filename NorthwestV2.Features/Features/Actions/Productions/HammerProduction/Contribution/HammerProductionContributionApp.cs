using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Contribution;

/// <summary>
/// Application service for contributing Time Points (TP) to a Hammer production.
/// Players can contribute TP to help complete a Hammer through its various stages.
/// </summary>
public class HammerProductionContributionApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HammerProductionContribution _hammerProductionContribution;
    private readonly IPlayerRepository _playerRepository;

    public HammerProductionContributionApp(IUnitOfWork unitOfWork,
        HammerProductionContribution hammerProductionContribution, IPlayerRepository playerRepository) : base(
        ActionNames.HammerProductionContribution)
    {
        _unitOfWork = unitOfWork;
        _hammerProductionContribution = hammerProductionContribution;
        _playerRepository = playerRepository;
    }

    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        InstantActionAvailability? instant = _hammerProductionContribution.DetermineAvailability(player);

        return instant;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        _hammerProductionContribution.Execute(player);

        await _unitOfWork.SaveChangesAsync();
    }
}