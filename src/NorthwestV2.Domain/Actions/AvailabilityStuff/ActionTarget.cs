namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

public class ActionTarget : IEquatable<ActionTarget>
{

    public string Name { get; set; } = "Not specified";
    /// <summary>
    /// Useful for targeting entities with Ids
    /// </summary>
    public Guid? TargetId { get; set; } = null;

    /// <summary>
    /// Useful for targeting values like quantities. 
    /// </summary>
    public string? Value { get; set; } = null;


    // TODO: a Compare() or override Equals() to check if one is equal to anotehr 

    public bool IsInvalidState => this.TargetId.HasValue && !string.IsNullOrEmpty(this.Value);

    public bool Equals(ActionTarget? other)
    {
        if (other is null) return false;
        if (IsInvalidState || other.IsInvalidState)
        {
            throw new Exception(
                "Both ways to define targets (value and id) cannot be defined at the same time when comparing equality");
        }

        if (ReferenceEquals(this, other)) return true;
        return Nullable.Equals(TargetId, other.TargetId) && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ActionTarget)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TargetId, Value);
    }
}