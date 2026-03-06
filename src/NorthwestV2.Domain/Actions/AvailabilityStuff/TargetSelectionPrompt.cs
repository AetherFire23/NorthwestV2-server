using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

public class TargetSelectionPrompt
{
    public required string Description { get; set; }

    public required List<ActionTarget> ValidTargets { get; set; } = [];

    public int Min { get; set; } = 1;
    public int Max { get; set; } = 1;
}