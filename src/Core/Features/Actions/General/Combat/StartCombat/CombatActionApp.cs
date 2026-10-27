using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat;

/// <summary>
/// Application-layer service for combat. Handles availability checking and execution.
/// Wraps <see cref="CombatAction"/> (domain logic) and handles persistence via repositories.
/// </summary>
public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly CombatAction _combatAction;
    private readonly CombatAftermath _combatAftermath;
    private readonly IUnitOfWork _unitOfWork;

    public CombatActionApp(IPlayerRepository playerRepository, CombatAction combatAction,
        CombatAftermath combatAftermath, IUnitOfWork unitOfWork) :
        base(ActionNames.CombatAction)
    {
        _playerRepository = playerRepository;
        _combatAction = combatAction;
        _combatAftermath = combatAftermath;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// When a player is in the same room. 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player caster = await _playerRepository.GetPlayerForCombat(request.PlayerId);

        List<Player> playersInSameRoom = await _playerRepository.GetPlayersInSameRoom(request.PlayerId);

        ActionWithTargetsAvailability combatActionAvailability =
            _combatAction.DetermineAvailability(caster, playersInSameRoom);

        return combatActionAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player attackerPlayer = await _playerRepository.GetPlayerForCombat(request.PlayerId);

        Guid defenderPlayerId = _combatAction.DetermineDefenderPlayer(request.ActionTargets);
        Player defenderPlayer = await _playerRepository.GetPlayerForCombat(defenderPlayerId);

        AttackerChoices choices = _combatAction.DetermineAttackerChoices(request.ActionTargets, attackerPlayer);

        // Everyone else conscious in the room sees the fight. Presence is the current room: there are no ticks yet.
        List<Player> witnesses = (await _playerRepository.GetPlayersInSameRoom(request.PlayerId))
            .Where(x => x.Id != defenderPlayerId && x.LifeStatus == PlayerLifeStatus.Alive)
            .ToList();

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attackerPlayer, defenderPlayer, choices);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, witnesses);
        AddReportsToGameLogs(reports);

        await _unitOfWork.SaveChangesAsync();
    }

    private static void AddReportsToGameLogs(List<CombatReport> reports)
    {
        foreach (CombatReport report in reports)
        {
            GameLog gameLog = new GameLog
            {
                Message = report.Message,
            };

            foreach (Player recipient in report.Recipients)
            {
                recipient.GameLogs.Add(gameLog);
            }
        }
    }
}
