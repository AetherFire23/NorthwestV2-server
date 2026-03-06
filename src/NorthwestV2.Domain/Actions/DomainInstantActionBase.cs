using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

namespace AetherFire23.ERP.Domain.Actions;

//TODO: Test this class very important 
public class GameActionsWithTargetsValidator
{
    // TODO: verify if I can move out the parameters and make it scoped instead 
    public void EnsureIsValidActionExecutionOrThrow(ActionWithTargetsAvailability actionWithTargetsAvailability,
        List<List<ActionTarget>> actionTargets)
    {
        if (!actionWithTargetsAvailability.CanExecute)
        {
            throw new Exception("Cannot execute because the number of targets");
        }

        if (!HasAllScreensInBetweenMinMaxBounds(actionWithTargetsAvailability, actionTargets))
        {
            throw new Exception("Cannot not be within min max bounds");
        }

        EnsureAllTargetsSelectionAreWithinValidTargets(actionWithTargetsAvailability, actionTargets);
        // TODO: Validate that all targets are valid
    }

    private bool HasAllScreensInBetweenMinMaxBounds(ActionWithTargetsAvailability actionWithTargetsAvailability,
        List<List<ActionTarget>> actionTargets)
    {
        bool isRespectingMinMaxBounds = true;
        for (var i = 0; i < actionWithTargetsAvailability.TargetSelectionPrompts.Count; i++)
        {
            TargetSelectionPrompt prompt = actionWithTargetsAvailability.TargetSelectionPrompts[i];

            List<ActionTarget> targetsSelectedForThisPrompt = actionTargets[i];

            bool isRespectingMinMaxBoundsForThisPrompt = targetsSelectedForThisPrompt.Count >= prompt.Min
                                                         && targetsSelectedForThisPrompt.Count <= prompt.Max;

            if (!isRespectingMinMaxBoundsForThisPrompt)
            {
                isRespectingMinMaxBounds = false;
            }
        }

        return isRespectingMinMaxBounds;
    }

    private void EnsureAllTargetsSelectionAreWithinValidTargets(
        ActionWithTargetsAvailability actionWithTargetsAvailability,
        List<List<ActionTarget>> actionTargets)
    {
        IEnumerable<ActionTarget> validTargets =
            actionWithTargetsAvailability.TargetSelectionPrompts.SelectMany(x => x.ValidTargets);

        IEnumerable<ActionTarget> selectedTargets = actionTargets.SelectMany(x => x);

        if (!selectedTargets.All(validTargets.Contains))
        {
            throw new Exception("Select targets are outside of bounds. ");
        }
    }
}