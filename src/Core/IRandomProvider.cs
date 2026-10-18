namespace NorthwestV2;

public interface IRandomProvider
{
    public int Next(int min, int max);

    public IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable);
}