using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class ChooseDefensiveCounterApp : ActionWithTargetsBase
{
    private readonly ChooseDefensiveCounter _chooseDefensiveCounter;

    public ChooseDefensiveCounterApp(NorthwestContext context, ChooseDefensiveCounter chooseDefensiveCounter) : base(
        context, ActionNames.PickDefensiveStance)
    {
        _chooseDefensiveCounter = chooseDefensiveCounter;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        TargetSelectionPrompt targetSelectionPrompt = CreatePromptOfDefensiveCounters();

        ActionWithTargetsAvailability stuff = new ActionWithTargetsAvailability
        {
            ActionName = ActionName,
            TargetSelectionPrompts = [targetSelectionPrompt]
        };

        int i = 0;
        return stuff;
    }

    private TargetSelectionPrompt CreatePromptOfDefensiveCounters()
    {
        List<ActionTarget> actionTargets = Enum.GetValues<DefensiveCounters>()
            .Select(x =>
                new ActionTarget()
                {
                    Value = x.ToString(),
                    Name = x.ToString(),
                    TargetId = Guid.Empty
                }).ToList();

        return new TargetSelectionPrompt()
        {
            Description = "Choose a defensive target stance.",
            ValidTargets = actionTargets
        };
    }

    /*
     * Execution part
     */

    public override async Task Execute(ExecuteActionRequest request)
    {
        DefensiveCounters defensiveCounter = ExtractDefensiveCounterFromActionTargets(request.ActionTargets);

        Player player = await Context.Players.FindById(request.PlayerId);

        _chooseDefensiveCounter.Execute(player, defensiveCounter);

        await Context.SaveChangesAsync();
    }

    private DefensiveCounters ExtractDefensiveCounterFromActionTargets(List<List<ActionTarget>> actionTargets)
    {
        if (actionTargets.Count > 0)
        {
            throw new Exception("Should not exceed 1 screen wth");
        }

        ActionTarget? target = actionTargets.FirstOrDefault()?.FirstOrDefault();

        if (target?.Value is null)
        {
            throw new Exception("Is null exception. ");
        }

        // Expects an enum.ToString()
        DefensiveCounters defensiveCounters = Enum.Parse<DefensiveCounters>(target.Value);

        return defensiveCounters;
    }
}