namespace NorthwestV2.Contexts;

public class MissingConnectionStringException : Exception
{
    public MissingConnectionStringException(string key)
        : base($"Connection string with key {key} not found in Configuration")
    {
    }
}
