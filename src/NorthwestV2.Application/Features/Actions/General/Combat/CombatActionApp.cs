using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Features.Actions.Core.TargetMapping;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly PlayerTargets _playerToTargets;

    public CombatActionApp(IPlayerRepository playerRepository, PlayerTargets playerToTargets) : base(ActionNames
        .CombatAction)
    {
        _playerRepository = playerRepository;
        _playerToTargets = playerToTargets;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        TargetSelectionPrompt promptOfPlayersAsTargets = await BuildPlayersAsTargetsPrompt(request.PlayerId);

        // TODO: COnsider to send NO targets if action requirements don't pass. (Should be enforced in base class)
        return ActionWithTargetsAvailability.Create(
            this.ActionName,
            ActionRequirement.None,
            promptOfPlayersAsTargets
        );
    }

    private async Task<TargetSelectionPrompt> BuildPlayersAsTargetsPrompt(Guid playerId)
    {
        List<Player> players = await _playerRepository.GetPlayersInSameRoom(playerId);
        List<ActionTarget> playersAsTargets = _playerToTargets.Convert(players);

        var prompt = new TargetSelectionPrompt()
        {
            Description = "Pick a player",
            ValidTargets = playersAsTargets,
        };

        return prompt;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}