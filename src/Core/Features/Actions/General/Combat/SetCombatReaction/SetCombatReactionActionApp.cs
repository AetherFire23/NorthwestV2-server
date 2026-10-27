using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.General.Combat.SetCombatReaction;

public class SetCombatReactionActionApp : ActionWithTargetsBase
{
    private readonly SetCombatReactionAction _setCombatReactionAction;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetCombatReactionActionApp(SetCombatReactionAction setCombatReactionAction,
        IPlayerRepository playerRepository, IUnitOfWork unitOfWork) : base(ActionNames.SetCombatReaction)
    {
        _setCombatReactionAction = setCombatReactionAction;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);
        List<Player> otherPlayers = await _playerRepository.GetOtherPlayersInGame(request.PlayerId);

        return _setCombatReactionAction.DetermineAvailability(player, otherPlayers);
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerForCombat(request.PlayerId);

        _setCombatReactionAction.Execute(player, request.ActionTargets);

        await _unitOfWork.SaveChangesAsync();
    }
}
