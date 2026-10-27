namespace NorthwestV2.Features.Shared;

/// <summary>
/// Health and Hope are never shown exactly, only in five 20% increments: 1-20 is 1, ..., 81-100 is 5. 0 is 0.
/// </summary>
public static class StatIncrements
{
    public const int INCREMENT_SIZE = 20;

    public static int From(int value)
    {
        int clamped = Math.Clamp(value, 0, 100);

        // Rounded up so that any value in 1-20 is the first increment.
        return (clamped + INCREMENT_SIZE - 1) / INCREMENT_SIZE;
    }
}
