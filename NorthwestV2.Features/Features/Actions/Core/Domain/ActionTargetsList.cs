using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;

namespace NorthwestV2.Features.Features.Actions.Domain.Core;

public class ActionTargetsList
{
    private List<List<ActionTarget>> _targets;

    private ActionTargetsList(List<List<ActionTarget>> targets)
    {
        _targets = targets;
    }

    public static ActionTargetsList From(List<List<ActionTarget>> actionTargets)
    {
        ActionTargetsList actionTargetsList = new ActionTargetsList(actionTargets);

        return actionTargetsList;
    }

    /// <summary>
    /// When an actionsTargetList has just one target, it should only have a single target.
    /// </summary>
    /// <returns></returns>
    public ActionTarget Single()
    {
        ActionTarget target = _targets.First().First();

        return target;
    }
}