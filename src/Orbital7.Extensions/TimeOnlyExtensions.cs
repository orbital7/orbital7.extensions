namespace Orbital7.Extensions;

public static class TimeOnlyExtensions
{
    public static string ToDefaultTimeString(
        this TimeOnly time)
    {
        return time.ToString(DateTimeHelper.TIME_FORMAT_DEFAULT);
    }

    public static string ToDefaultTime24HourString(
        this TimeOnly time)
    {
        return time.ToString(DateTimeHelper.TIME_FORMAT_24_HOUR_DEFAULT);
    }
}
