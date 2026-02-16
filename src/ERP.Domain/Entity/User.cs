using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class User : EntityBase
{
    public required string Username { get; set; } = string.Empty;
    public required string HashedPassword { get; set; }

    public static User Create(string username, string password, Guid companyId)
    {

        var user = new User
        {
            Username = username,
            HashedPassword = password // TODO: HASH PASSWORD LOL 
        };

        return user;
    }
}