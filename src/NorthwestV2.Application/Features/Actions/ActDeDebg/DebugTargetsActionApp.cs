using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.Debg;
using AetherFire23.ERP.Domain.Features.Actions.Debug;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.ActDeDebg;

public class DebugTargetsActionApp : ActionWithTargetsBase
{
    private IPlayerRepository _playerRepository;
    private readonly DebugTargetAction _debugTargetAction;

    public DebugTargetsActionApp(DebugTargetAction debugTargetAction, IPlayerRepository playerRepository) : base(
        ActionNames.DebugWithTargets)
    {
        _debugTargetAction = debugTargetAction;
        _playerRepository = playerRepository;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player caster = await _playerRepository.GetPlayer(request.PlayerId);

        List<Player> otherPlayers = await _playerRepository.GetPlayersInSameGame(request.PlayerId);

        ActionWithTargetsAvailability availability = _debugTargetAction.GetAvailability(caster, otherPlayers);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
    }
}