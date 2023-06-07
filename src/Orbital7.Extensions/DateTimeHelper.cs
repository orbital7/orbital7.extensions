namespace System;

public static class DateTimeHelper
{
    public static TimeZoneInfo GetTimeZone(string timeZoneId)
    {
        if (!string.IsNullOrEmpty(timeZoneId))
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        else
            return TimeZoneInfo.Utc;
    }
}
