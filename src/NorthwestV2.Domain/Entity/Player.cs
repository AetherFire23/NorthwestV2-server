using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;


// TODO: Consider using a base class for this 
public class Player : EntityBase
{
    public Guid UserId { get; set; }
    public required User User { get; set; }
    
    // maybe do a UserFactory instead so that it can consume a RandomProvider and get some role ? 
    public Player CreatePlayer(User user)
    {
        Player player = new Player()
        {
            User = user,
        };

        return player;
    }
}