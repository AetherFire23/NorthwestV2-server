using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Actions.Bases;

//TODO: Test this class very important 
public class GameActionsWithTargetsValidator
{
    public void EnsureIsValidActionExecutionOrThrow(ActionWithTargetsAvailability actionWithTargetsAvailability,
        List<List<ActionTarget>> actionTargets)
    {
        if (actionWithTargetsAvailability.TargetSelectionPrompts.Count != actionTargets.Count)
        {
            throw new Exception(
                $"There is not the same amount of prompts in each screens {actionWithTargetsAvailability.TargetSelectionPrompts.Count} / {actionTargets.Count}");
        }

        if (!actionWithTargetsAvailability.CanExecute)
        {
            throw new Exception("Cannot execute because the number of targets");
        }

        if (!HasAllScreensInBetweenMinMaxBounds(actionWithTargetsAvailability, actionTargets))
        {
            throw new Exception("Cannot not be within min max bounds");
        }

        EnsureAllTargetsSelectionAreWithinValidProvidedTargets(actionWithTargetsAvailability, actionTargets);
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

    private void EnsureAllTargetsSelectionAreWithinValidProvidedTargets(
        ActionWithTargetsAvailability actionWithTargetsAvailability,
        List<List<ActionTarget>> actionTargets)
    {
        // Iterating through the valid targets and the selected targets at the same time.
        // Valid targets for screen at i -> selections for screen at i
        for (var i = 0; i < actionWithTargetsAvailability.TargetSelectionPrompts.Count; i++)
        {
            TargetSelectionPrompt prompt = actionWithTargetsAvailability.TargetSelectionPrompts[i];

            List<ActionTarget> selectedTargets = actionTargets[i];

            if (!selectedTargets.All(prompt.ValidTargets.Contains))
            {
                throw new Exception("Select targets are outside of bounds. ");
            }
        }
    }
}