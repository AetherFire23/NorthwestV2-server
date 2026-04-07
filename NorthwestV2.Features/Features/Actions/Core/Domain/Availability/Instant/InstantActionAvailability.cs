using NorthwestV2.Features.Features.Actions.Domain.Core.Availability;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;

/// <summary>
/// Represents the availability of a game action that can be executed immediately and doesn't require any task selection
/// 
/// <para>
/// This class inherits from <see cref="ActionAvailabilityBase"/> and does not
/// introduce additional behavior. 
/// </para>
/// </summary>
public class InstantActionAvailability : ActionAvailabilityBase
{
}