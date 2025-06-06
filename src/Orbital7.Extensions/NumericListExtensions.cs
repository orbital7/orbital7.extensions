using System.Numerics;

namespace Orbital7.Extensions;

public static class NumericListExtensions
{
    public static List<int> GetConsectutiveItemCounts<TModel>(
        this IEnumerable<TModel?> list,
        Func<TModel, bool> itemSelector)
    {
        var consectutiveCounts = new List<int>();

        var consectutiveCount = 0;
        foreach (var item in list)
        {
            if (item != null && itemSelector(item))
            {
                consectutiveCount++;
            }
            else
            {
                consectutiveCounts.Add(consectutiveCount);
                consectutiveCount = 0;
            }
        }

        return consectutiveCounts;
    }

    public static List<TValue> GetConsectutiveItemValueSums<TModel, TValue>(
        this IEnumerable<TModel?> list,
        Func<TModel, bool> itemSelector,
        Func<TModel, TValue> valueSelector)
        where TValue : INumber<TValue>
    {
        var consectutiveValues = new List<TValue>();

        var consectutiveValue = TValue.Zero;
        foreach (var item in list)
        {
            if (item != null && itemSelector(item))
            {
                consectutiveValue += valueSelector(item);
            }
            else
            {
                consectutiveValues.Add(consectutiveValue);
                consectutiveValue = TValue.Zero;
            }
        }

        return consectutiveValues;
    }

    public static TValue? MaxOrDefault<TValue>(
        this IEnumerable<TValue> values)
        where TValue : struct, IComparable<TValue>
    {
        if (!values.Any())
        {
            return default;
        }

        return values.Max();
    }

    public static TValue? MaxOrDefault<TModel, TValue>(
        this IEnumerable<TModel> list,
        Func<TModel, TValue> valueSelector)
        where TValue : struct, IComparable<TValue>
    {
        return list
            .Select(valueSelector)
            .MaxOrDefault();
    }


    public static TValue? MinOrDefault<TValue>(
        this IEnumerable<TValue> values)
        where TValue : struct, IComparable<TValue>
    {
        if (!values.Any())
        {
            return default;
        }

        return values.Min();
    }

    public static TValue? MinOrDefault<TModel, TValue>(
        this IEnumerable<TModel> list,
        Func<TModel, TValue> valueSelector)
        where TValue : struct, IComparable<TValue>
    {
        return list
            .Select(valueSelector)
            .MinOrDefault();
    }

    public static double? AverageOrDefault(
        this IEnumerable<double> values)
    {
        if (!values.Any())
        {
            return default;
        }

        return values.Average();
    }

    public static double? AverageOrDefault<T>(
        this IEnumerable<T> items,
        Func<T, double> valueSelector)
    {
        return items
            .Select(valueSelector)
            .AverageOrDefault();
    }

    public static decimal? AverageOrDefault(
        this IEnumerable<decimal> values)
    {
        if (!values.Any())
        {
            return default;
        }

        return values.Average();
    }

    public static decimal? AverageOrDefault<T>(
        this IEnumerable<T> items,
        Func<T, decimal> valueSelector)
    {
        return items
            .Select(valueSelector)
            .AverageOrDefault();
    }

    public static TValue? MedianOrDefault<TValue>(
        this IEnumerable<TValue> values)
        where TValue : struct, INumber<TValue>, IComparable<TValue>
    {
        if (!values.Any())
        {
            return default;
        }

        List<TValue> sortedValues = values
            .OrderBy(x => x)
            .ToList();

        int midpoint = sortedValues.Count / 2;

        if (sortedValues.Count.IsEven())
        {
            // Even number of elements: average the two middle values
            return (sortedValues[midpoint - 1] + sortedValues[midpoint]) / TValue.CreateChecked(2);
        }
        else
        {
            // Odd number of elements: return the middle value
            return sortedValues[midpoint];
        }
    }

    public static TValue? MedianOrDefault<TModel, TValue>(
        this IEnumerable<TModel> list,
        Func<TModel, TValue> valueSelector)
        where TValue : struct, INumber<TValue>, IComparable<TValue>
    {
        return list
            .Select(valueSelector)
            .MedianOrDefault();
    }

    public static double? StandardDeviation<T>(
        this IEnumerable<T> values)
        where T : struct, IConvertible
    {
        return CalculateStandardDeviation(
            values,
            isSample: true);
    }

    public static double? PopulationStandardDeviation<T>(
        this IEnumerable<T> values)
        where T : struct, IConvertible
    {
        return CalculateStandardDeviation(
            values,
            isSample: false);
    }

    private static double? CalculateStandardDeviation<T>(
        IEnumerable<T> values,
        bool isSample)
        where T : struct, IConvertible
    {
        var doubleValues = values.Select(x => Convert.ToDouble(x)).ToList();

        if (doubleValues.Count > 0)
        {
            double mean = doubleValues.Average();
            double sumOfSquares = doubleValues.Sum(v => Math.Pow(v - mean, 2));
            int divisor = isSample ? doubleValues.Count - 1 : doubleValues.Count;

            if (divisor > 0)
            {
                return Math.Sqrt(sumOfSquares / divisor);
            }
        }

        return null;
    }
}
