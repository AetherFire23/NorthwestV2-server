using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Lobby : EntityBase
{
    public ICollection<User> Users { get; set; } = [];

    public static Lobby Create(User creator)
    {
        Lobby lobby = new Lobby();
        lobby.Users.Add(creator);
        return lobby;
    }
}