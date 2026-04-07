using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions;

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

        _cancelProductionAction.CancelProduction(player, player.Room, ActionTargetsList.From(request.ActionTargets));
        
        // TODO:
        // itemrepository.DeleteItem();
        
        await _unitOfWork.SaveChangesAsync();
    }
}