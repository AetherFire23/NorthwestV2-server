using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class ChooseDefensiveCounterApp : ActionWithTargetsBase
{
    private readonly ChooseDefensiveCounter _chooseDefensiveCounter;

    public ChooseDefensiveCounterApp(NorthwestContext context, string actionName,
        ChooseDefensiveCounter chooseDefensiveCounter) : base(context, actionName)
    {
        _chooseDefensiveCounter = chooseDefensiveCounter;
    }

    public override Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        throw new NotImplementedException();
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        DefensiveCounters defensiveCounter = ExtractDefensiveCounterFromActionTargets(request.ActionTargets);

        throw new NotImplementedException();
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