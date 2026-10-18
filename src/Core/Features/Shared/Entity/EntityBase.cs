using System.ComponentModel.DataAnnotations;

namespace NorthwestV2.Features.Features.Shared.Entity;

/// <summary>
/// Base class for all domain entities. Uses <see cref="Id"/> for equality (entity identity pattern).
/// Two entities are equal if they have the same GUID, regardless of other property values.
/// </summary>
public class EntityBase
{
    [Key] public Guid Id { get; set; } = Guid.Empty;

    public override bool Equals(object? obj)
    {
        // If it is entityBase AND they have the same Id. 
        if (obj is EntityBase other)
        {
            return other.Id.Equals(this.Id);
        }

        return false;
    }

    public bool Equals(EntityBase other)
    {
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        /* ReSharper disable once NonReadonlyMemberInGetHashCode
         I need it to be a property for any kind of EfCore recognition / injection. but it make it so hashcode cries.
         Anyways it can't really change since it is auto-initialized
         */
        return Id.GetHashCode();
    }
}