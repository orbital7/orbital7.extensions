namespace Orbital7.Extensions;

public static class DateTimeHelper
{
    public const string DEFAULT_DATE_FORMAT = "MM/dd/yyyy";

    public static TimeZoneInfo GetTimeZone(
        string timeZoneId)
    {
        if (!string.IsNullOrEmpty(timeZoneId))
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        else
            return TimeZoneInfo.Utc;
    }
}