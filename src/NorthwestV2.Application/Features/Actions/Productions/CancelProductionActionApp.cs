using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.Productions;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions;

public class CancelProductionActionApp : ActionWithTargetsBase
{
    private readonly CancelProductionAction _cancelProductionAction;
    private readonly IPlayerRepository _playerRepository;

    private IUnitOfWork _unitOfWork;

    public CancelProductionActionApp(CancelProductionAction cancelProductionAction, IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork)
        : base(ActionNames.CancelProduction)
    {
        _cancelProductionAction = cancelProductionAction;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        ActionWithTargetsAvailability cancelProductionAvailability = _cancelProductionAction.GetAvailability(player);

        return cancelProductionAvailability;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        _cancelProductionAction.CancelProduction(player, ActionTargetsList.From(request.ActionTargets));

        await _unitOfWork.SaveChangesAsync();
    }
}