namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

// TODO: Even more complex than expected. 
// need to check if the actual required target is the Id or the value.
// I had a bug wher eI needed to use always an Id because I didnt take this into account
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