using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.Domain.General.Combat;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat;

public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly CombatAction _combatAction;

    public CombatActionApp(IPlayerRepository playerRepository, CombatAction combatAction) :
        base(ActionNames.CombatAction)
    {
        _playerRepository = playerRepository;
        _combatAction = combatAction;
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
        throw new NotImplementedException();
    }
}