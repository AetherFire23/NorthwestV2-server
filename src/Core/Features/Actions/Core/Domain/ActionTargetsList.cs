using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;

namespace NorthwestV2.Features.Actions.Core.Domain;

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
    /// The single target selected for the prompt at <paramref name="promptIndex"/>.
    /// </summary>
    public ActionTarget At(int promptIndex)
    {
        ActionTarget target = _targets[promptIndex].First();

        return target;
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