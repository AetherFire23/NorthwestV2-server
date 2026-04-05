using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Contribution;

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