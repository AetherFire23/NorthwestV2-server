using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution;

/// <summary>
/// Contributing to a spyglass asks two things: which unfinished spyglass of the room, and which specialized TP quota
/// to pay. Most stages have a single quota; the second one has Scholar and Sentry.
/// </summary>
public class SpyglassProductionContributionAction
{
    public const int SPYGLASS_PROMPT_INDEX = 0;
    public const int QUOTA_PROMPT_INDEX = 1;

    public ActionWithTargetsAvailability? DetermineAvailability(Player player)
    {
        List<UnfinishedSpyglass> unfinishedSpyglasses = GetUnfinishedSpyglasses(player);
        if (unfinishedSpyglasses.Count == 0)
        {
            return null;
        }

        /*
         * The requirements describe the first spyglass of the room; the prompts let the player pick any of them.
         * Contribute() rejects a quota the picked spyglass does not accept.
         */
        StageContributionBase stage = unfinishedSpyglasses.First().CurrentStageContribution;

        List<Roles> openRoles = unfinishedSpyglasses
            .SelectMany(x => x.CurrentStageContribution.ContributionRoles
                .Where(role => x.CurrentStageContribution.IsQuotaOpen(role)))
            .Distinct()
            .ToList();

        // The cheapest open quota is the one that tells whether the player can afford to contribute at all.
        Roles referenceRole = openRoles
            .OrderBy(role => stage.CalculateCostForContribution(player, role))
            .DefaultIfEmpty(stage.RequiredRole)
            .First();

        List<ActionRequirement> requirements = stage.GetRequirements(player, referenceRole);
        requirements.Add(new ActionRequirement("At least one TP quota of the spyglass is still open.",
            openRoles.Count > 0));

        TargetSelectionPrompt spyglassPrompt = TargetSelectionPrompt.FromItems(
            unfinishedSpyglasses.Cast<ItemBase>().ToList(), "Choose the unfinished spyglass to contribute to.");

        TargetSelectionPrompt quotaPrompt = new()
        {
            Description = "Choose the specialized TP to contribute.",
            ValidTargets = openRoles.Select(CreateQuotaTarget).ToList(),
        };

        ActionWithTargetsAvailability availability = new()
        {
            ActionName = ActionNames.SpyglassContribution,
            DisplayName = stage.StageName,
            ActionRequirements = requirements,
            TargetSelectionPrompts = [spyglassPrompt, quotaPrompt],
        };

        return availability;
    }

    public void Contribute(Player player, ActionTargetsList actionTargets)
    {
        ActionTarget spyglassTarget = actionTargets.At(SPYGLASS_PROMPT_INDEX);
        ActionTarget quotaTarget = actionTargets.At(QUOTA_PROMPT_INDEX);

        UnfinishedSpyglass unfinishedSpyglass = (UnfinishedSpyglass)player.Room.Inventory
            .FindById(spyglassTarget.TargetId!.Value);
        Roles quotaRole = Enum.Parse<Roles>(quotaTarget.Value!);

        unfinishedSpyglass.Contribute(player, quotaRole);
    }

    private static List<UnfinishedSpyglass> GetUnfinishedSpyglasses(Player player)
    {
        return player.Room.Inventory.Items.OfType<UnfinishedSpyglass>().ToList();
    }

    private static ActionTarget CreateQuotaTarget(Roles role)
    {
        return new ActionTarget
        {
            Name = $"{role} TP",
            Value = role.ToString(),
            TargetId = Guid.Empty,
        };
    }
}
