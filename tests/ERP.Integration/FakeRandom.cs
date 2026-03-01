using AetherFire23.ERP.Domain;

namespace NorthwestV2.Integration;

public class FakeRandom : IRandomProvider
{
    private readonly IEnumerator<int> _enumerator;

    public FakeRandom(List<int> integers)
    {
        _enumerator = integers.GetEnumerator();
    }

    public int Next(int min, int max)
    {
        _enumerator.MoveNext();
        int currVal = _enumerator.Current;
        return currVal;
    }

    public IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable)
    {
        List<T> copy = new List<T>(enumerable);
        List<T> tList = new List<T>();

        while (copy.Count != 0)
        {
            _enumerator.MoveNext();
            int currVal = _enumerator.Current;

            T element = enumerable.ElementAt(currVal);
            tList.Add(element);
            copy.Remove(element);
        }

        return tList;
    }
}