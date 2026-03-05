namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

public class ActionTarget
{
    /// <summary>
    /// Useful for targeting entities with Ids
    /// </summary>
    public Guid? TargetId { get; set; }

    /// <summary>
    /// Useful for targeting values like quantities. 
    /// </summary>
    public string? Value { get; set; }
}