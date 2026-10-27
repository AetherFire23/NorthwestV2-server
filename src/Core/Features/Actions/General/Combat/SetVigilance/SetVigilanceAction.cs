using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.SetVigilance;

/// <summary>
/// Turns the secret, persistent vigilance setting on or off. A player who is not vigilant is surprised when attacked.
/// The Sentry is always vigilant whatever he sets here.
/// </summary>
public class SetVigilanceAction
{
    public ActionWithTargetsAvailability DetermineAvailability()
    {
        ActionWithTargetsAvailability availability = new()
        {
            ActionName = ActionNames.SetVigilance,
            DisplayName = "Set vigilance",
            TargetSelectionPrompts = [TargetSelectionPrompt.FromEnum<VigilanceSettings>("Be vigilant?")],
        };

        return availability;
    }

    public void Execute(Player player, List<List<ActionTarget>> actionTargets)
    {
        string value = actionTargets.First().First().Value
                       ?? throw new Exception("A vigilance setting must be selected.");

        player.IsVigilant = Enum.Parse<VigilanceSettings>(value) == VigilanceSettings.Vigilant;
    }
}
