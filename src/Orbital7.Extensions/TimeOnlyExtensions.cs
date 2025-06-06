namespace Orbital7.Extensions;

public static class TimeOnlyExtensions
{
    public static string ToDefaultTimeString(
        this TimeOnly time)
    {
        return time.ToString("h:mm tt");
    }

    public static string? ToDefaultTimeString(
        this TimeOnly? time,
        string? nullValue = null)
    {
        if (time.HasValue)
        {
            return time.Value.ToDefaultTimeString();
        }
        else
        {
            return nullValue;
        }
    }
}
