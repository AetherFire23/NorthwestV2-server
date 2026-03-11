using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class CombatActionApp : ActionWithTargetsBase
{
    private readonly IPlayerRepository _playerRepository;

    public CombatActionApp(IPlayerRepository playerRepository) : base(ActionNames.CombatAction)
    {
        _playerRepository = playerRepository;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        var players = await _playerRepository.GetPlayersInSameGame(request.PlayerId);
        return new ActionWithTargetsAvailability
        {
            ActionName = this.ActionName,
            ActionRequirements = ActionRequirement.None,
            TargetSelectionPrompts =
            [
                new TargetSelectionPrompt
                {
                    Description = "Picka a player Target",
                    ValidTargets =
                    [
                        new ActionTarget
                        {
                            Name = "Convert all player to targets here"
                        }
                    ],
                }
            ]
        };
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}