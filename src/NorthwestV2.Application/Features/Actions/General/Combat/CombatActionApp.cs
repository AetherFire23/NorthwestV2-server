using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly CombatAction _combatAction;

    public CombatActionApp(IPlayerRepository playerRepository, CombatAction combatAction) :
        base(ActionNames
            .CombatAction)
    {
        _playerRepository = playerRepository;
        _combatAction = combatAction;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
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