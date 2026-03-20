using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

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