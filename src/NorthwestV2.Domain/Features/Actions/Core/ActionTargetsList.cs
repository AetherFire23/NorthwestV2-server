using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Features.Actions.Core;

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