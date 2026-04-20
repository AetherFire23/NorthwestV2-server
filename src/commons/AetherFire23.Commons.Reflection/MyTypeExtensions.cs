namespace AetherFire23.Commons.Reflection;

public static class MyTypeExtensions
{
    public static Type GetBaseTypeOrThrow(this Type t)
    {
        Type? baseType = t.BaseType;

        return baseType ?? throw new Exception("Base type could not be found.");
    }
}