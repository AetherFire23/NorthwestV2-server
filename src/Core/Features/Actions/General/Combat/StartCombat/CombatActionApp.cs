using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat;

/// <summary>
/// Application-layer service for combat. Handles availability checking and execution.
/// Wraps <see cref="CombatAction"/> (domain logic) and handles persistence via repositories.
/// </summary>
public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly CombatAction _combatAction;
    private readonly IUnitOfWork _unitOfWork;

    public CombatActionApp(IPlayerRepository playerRepository, CombatAction combatAction, IUnitOfWork unitOfWork) :
        base(ActionNames.CombatAction)
    {
        _playerRepository = playerRepository;
        _combatAction = combatAction;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// When a player is in the same room. 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player caster = await _playerRepository.GetPlayer(request.PlayerId);

        List<Player> playersInSameRoom = await _playerRepository.GetPlayersInSameRoom(request.PlayerId);

        ActionWithTargetsAvailability combatActionAvailability =
            _combatAction.DetermineAvailability(caster, playersInSameRoom);

        return combatActionAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        // Get attacker
        Player attackerPlayer = await _playerRepository.GetPlayer(request.PlayerId);


        // get defender
        Guid defenderPlayerId = _combatAction.DetermineDefenderPlayer(request.ActionTargets);
        Player player = await _playerRepository.GetPlayer(defenderPlayerId);

        // Determine AttackerStance 

        AttackerStances attackerStance = _combatAction.DetermineAttackerStance(request.ActionTargets);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attackerPlayer, player, attackerStance);
        
        

        await _unitOfWork.SaveChangesAsync();
    }
}