namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;

/// <summary>
/// Represents a target for an action, which may be identified either by a
/// <see cref="Guid"/> identifier or by a string value. Only one form of
/// identification may be used at a time.
/// </summary>
/// <remarks>
/// An <see cref="ActionTarget"/> can be defined in two mutually exclusive ways:
/// <list type="bullet">
/// <item><description><see cref="TargetId"/> — a strongly typed identifier.</description></item>
/// <item><description><see cref="Value"/> — a free‑form string representation.</description></item>
/// </list>
/// 
/// <para>
/// If both are set simultaneously, the instance is considered invalid and
/// <see cref="IsInvalidState"/> will return <c>true</c>.  
/// Equality checks will throw if either operand is in an invalid state.
/// </para>
/// 
/// <para>
/// Equality is based on whichever representation is used:
/// <list type="bullet">
/// <item>Two targets with matching <see cref="TargetId"/> values are equal.</item>
/// <item>Two targets with matching <see cref="Value"/> values are equal.</item>
/// </list>
/// </para>
/// </remarks>
public class ActionTarget : IEquatable<ActionTarget>
{
    public string Name { get; set; } = "Not specified";


    /// <summary>
    /// The identifier of the target, if the target is represented by a GUID.
    /// Mutually exclusive with <see cref="Value"/>.
    public Guid? TargetId { get; set; } = null;

    /// <summary>
    /// A string representation of the target, used when no GUID identifier applies.
    /// Mutually exclusive with <see cref="TargetId"/>.
    /// </summary>
    public string? Value { get; set; } = null;

    /// <summary>
    /// Indicates whether the target is in an invalid state, meaning both
    /// <see cref="TargetId"/> and <see cref="Value"/> are set simultaneously.
    /// </summary>
    public bool IsInvalidState => (this.TargetId.HasValue && this.TargetId != Guid.Empty) && !string.IsNullOrEmpty(this.Value);

    /// <summary>
    /// Determines whether this target is equal to another target.
    /// Throws if either target is in an invalid state.
    /// </summary>
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


    public override string ToString()
    {
        return $"{this.Name} - {this.Value ?? ""} {this.TargetId}";
    }
}