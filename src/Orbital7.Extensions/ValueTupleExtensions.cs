namespace Orbital7.Extensions;

public static class ValueTupleExtensions
{
    public static T2 GetItem2<T1, T2>(
        this List<(T1, T2)> list,
        T1 item1)
    {
        return (from x in list
                where x.Item1 == null && item1 == null || x.Item1 != null && x.Item1.Equals(item1)
                select x.Item2).SingleOrDefault();
    }

    public static List<T2> GatherItem2s<T1, T2>(
        this List<(T1, T2)> list,
        T1 item1)
    {
        return (from x in list
                where x.Item1 == null && item1 == null || x.Item1 != null && x.Item1.Equals(item1)
                select x.Item2).ToList();
    }

    public static T1 GetItem1<T1, T2>(
        this List<(T1, T2)> list,
        T2 item2)
    {
        return (from x in list
                where x.Item2 == null && item2 == null || x.Item2 != null && x.Item2.Equals(item2)
                select x.Item1).SingleOrDefault();
    }

    public static List<T1> GatherItem1s<T1, T2>(
        this List<(T1, T2)> list,
        T2 item2)
    {
        return (from x in list
                where x.Item2 == null && item2 == null || x.Item2 != null && x.Item2.Equals(item2)
                select x.Item1).ToList();
    }

    public static bool HasItem1<T1, T2>(
        this List<(T1, T2)> list,
        T1 item1)
    {
        return (from x in list
                where x.Item1 == null && item1 == null ||
                      x.Item1 != null && x.Item1.Equals(item1)
                select x).Count() > 0;
    }

    public static bool HasItem2<T1, T2>(
        this List<(T1, T2)> list,
        T2 item2)
    {
        return (from x in list
                where x.Item2 == null && item2 == null ||
                      x.Item2 != null && x.Item2.Equals(item2)
                select x).Count() > 0;
    }
}
