using System.ComponentModel.DataAnnotations;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;

/// <summary>
/// Allo am I seen 
/// </summary>
public class ActionWithTargetsAvailability : ActionAvailabilityBase
{
    /// <summary>
    /// It may be normal that a TagetSelectionPrompts is empty if some aciton requirement is invalid. 
    /// </summary>
    [Required]
    public List<TargetSelectionPrompt> TargetSelectionPrompts { get; set; } = [];


    public ActionWithTargetsAvailability WithScreen(TargetSelectionPrompt nextPrompt)
    {
        this.TargetSelectionPrompts.Add(nextPrompt);

        return this;
    }

    public static ActionWithTargetsAvailability Create(string name, List<ActionRequirement> req,
        TargetSelectionPrompt prompt)
    {
        ActionWithTargetsAvailability action = new ActionWithTargetsAvailability()
        {
            ActionName = name,
            DisplayName = name
        };

        action.ActionRequirements.AddRange(req);
        action.TargetSelectionPrompts.Add(prompt);


        return action;
    }
}