using System.Text.Json;

namespace NortwestV2.Api;

public static class SessionExtensions
{
    private const string UserDataKey = "userData";

    public static void SetUserData(this ISession session, UserData data)
    {
        var json = JsonSerializer.Serialize(data);
        session.SetString(UserDataKey, json);
    }

    public static UserData GetUserData(this ISession session)
    {
        var json = session.GetString(UserDataKey);

        if (json is null)
            throw new Exception("UserData should exist in this session.");

        var data = JsonSerializer.Deserialize<UserData>(json);

        if (data is null)
            throw new Exception("UserData could not be deserialized.");

        return data;
    }

    public static bool HasUserData(this ISession session)
    {
        return session.GetString(UserDataKey) is not null;
    }

    public static void ClearUserData(this ISession session)
    {
        session.Remove(UserDataKey);
    }
}

public class UserData
{
    public Guid? UserId { get; set; }
    public Guid? PlayerId { get; set; }
    public Guid? GameId { get; set; }
}