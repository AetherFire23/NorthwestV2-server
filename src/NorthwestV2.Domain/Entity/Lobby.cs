using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Lobby : EntityBase
{
    public ICollection<User> Users { get; set; } = [];
}