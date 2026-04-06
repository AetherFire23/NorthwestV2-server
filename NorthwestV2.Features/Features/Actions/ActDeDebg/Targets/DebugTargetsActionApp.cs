using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.ActDeDebg.Targets;

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