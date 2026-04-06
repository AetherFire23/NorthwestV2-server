namespace NorthwestV2.Features;

public class RealRandom : IRandomProvider
{
    public int Next(int min, int max)
    {
        return Random.Shared.Next(min, max);
    }

    public IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable)
    {
        List<T> copy = new List<T>(enumerable);
        List<T> shuffled = new List<T>();

        while (copy.Count != 0)
        {
            int index = Random.Shared.Next(0, copy.Count);

            shuffled.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return shuffled;
    }
}