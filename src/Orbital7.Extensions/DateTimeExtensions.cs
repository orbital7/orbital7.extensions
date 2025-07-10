using Orbital7.Extensions.Data;

namespace Orbital7.Extensions;

public enum Month
{
    January = 0,
    February = 1,
    March = 2,
    April = 3,
    May = 4,
    June = 5,
    July = 6,
    August = 7,
    September = 8,
    October = 9,
    November = 10,
    December = 11,
}

public static class DateTimeExtensions
{
    public static DateTime UtcToTimeZone(
        this DateTime dateTimeUtc, 
        TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc), timeZone);
    }

    public static DateTime TimeZoneToUtc(
        this DateTime dateTimeInTimeZone,
        TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTime(dateTimeInTimeZone, timeZone, TimeZoneInfo.Utc);
    }

    public static string ToHoursMinutesString(
        this TimeSpan ts,
        bool includeSecondsIfLessThanOneMinute = false,
        bool includeSecondsIfZero = false)
    {
        if (includeSecondsIfLessThanOneMinute &&
            (int)ts.TotalHours == 0 &&
            ts.Minutes == 0 &&
            (includeSecondsIfZero || ts.Seconds != 0))
        {
            return ts.ToHoursMinutesSecondsString();
        }
        else
        {
            return String.Format("{0:00}:{1:00}", (int)ts.TotalHours, ts.Minutes);
        }
    }

    public static string ToHoursMinutesSecondsString(
        this TimeSpan ts)
    {
        return String.Format("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
    }

    public static DateTimeSpan CalulateDateTimeSpan(
        this DateTime dateTime, 
        DateTime dateToCompare)
    {
        return DateTimeSpan.CompareDates(dateTime, dateToCompare);
    }

    public static int CalculateStandardFiscalQuartersDifference(
        this DateTime dateTime, 
        DateTime dateToCompare)
    {
        return ((dateTime.Year - dateToCompare.Year) * 4) + dateTime.GetStandardFiscalQuarter() - dateToCompare.GetStandardFiscalQuarter();
    }

    public static double CalculateMonthsDifference(
        this DateTime dateTime, 
        DateTime dateTimeToCompare)
    {
        return dateTime.Subtract(dateTimeToCompare).TotalDays / DateTimeHelper.GetAverageDaysPerMonth();
    }

    public static string ToDefaultDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.DATE_FORMAT_DEFAULT);
    }
    
    public static string ToDefaultTimeString(
        this DateTime dateTime)
    {
        // TODO: Not liking the am/pm without a space, and
        // the conditional seconds are a problem; go back
        // to default for now.
        //
        //var fi = new DateTimeFormatInfo()
        //{
        //    AMDesignator = "am",
        //    PMDesignator = "pm"
        //};
        //
        //var format = dateTime.Second == 0 ?
        //    "h:mmtt" :
        //    "h:mm:sstt";
        //return dateTime.ToString(format, fi);

        return dateTime.ToString(DateTimeHelper.TIME_FORMAT_DEFAULT);
    }

    public static string ToDefaultTime24HourString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.TIME_FORMAT_24_HOUR_DEFAULT);
    }

    public static string ToDefaultDateTimeString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.DATE_TIME_FORMAT_DEFAULT);
    }

    public static string ToDefaultDateTime24HourString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.DATE_TIME_FORMAT_DEFAULT_24_HOUR);
    }

    public static string ToLongMonthYearDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.DATE_FORMAT_MONTH_YEAR_LONG);
    }

    public static string ToNaiveDateTimeString(
        this DateTime dateTime)
    {
        return string.Format("{0:yyyy-MM-ddTHH:mm:ss}", dateTime);
    }

    public static string ToISO8601DateTimeString(
        this DateTime dateTimeUtc)
    {
        return string.Format("{0:yyyy-MM-ddTHH:mm:ss}Z", dateTimeUtc);
    }

    public static string ToISO8601DateString(
        this DateTime dateTimeUtc)
    {
        return dateTimeUtc.ToString(DateTimeHelper.DATE_FORMAT_YEAR_MONTH_DAY_DASHED);
    }

    public static DateTime EnsureNotFutureDateTimeUtc(
        this DateTime dateTimeUtc,
        DateTime? nowUtc = null)
    {
        var myNowUtc = nowUtc ?? DateTime.UtcNow;
        if (dateTimeUtc > myNowUtc)
        {
            dateTimeUtc = myNowUtc;
        }

        return dateTimeUtc;
    }

    public static DateTime RoundUpToStartOfBusinessDay(
        this DateTime dateTime)
    {
        if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            dateTime = dateTime.AddDays(2);
        else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            dateTime = dateTime.AddDays(1);

        return dateTime.RoundDownToStartOfDay();
    }

    public static DateTime RoundUpToStartOfNextBusinessDay(
        this DateTime dateTime)
    {
        return dateTime.AddDays(1).RoundUpToStartOfBusinessDay();
    }

    public static DateTime RoundDownToStartOfSecond(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
    }

    public static DateTime RoundUpToEndOfSecond(
        this DateTime dateTime)
    {
        return dateTime.RoundDownToStartOfSecond().AddSeconds(1).Subtract(Get1Microsecond());
    }

    public static DateTime RoundDownToStartOfMinute(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);
    }

    public static DateTime RoundUpToEndOfMinute(
        this DateTime dateTime)
    {
        return dateTime.RoundDownToStartOfMinute().AddMinutes(1).Subtract(Get1Microsecond());
    }

    public static DateTime RoundDownToStartOfHour(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind);
    }

    public static DateTime RoundUpToEndOfHour(
        this DateTime dateTime)
    {
        return dateTime.RoundDownToStartOfHour().AddHours(1).Subtract(Get1Microsecond());
    }

    public static DateTime RoundDownToStartOfDay(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static DateTime RoundUpToEndOfDay(
        this DateTime dateTime)
    {
        return dateTime.RoundDownToStartOfDay().AddDays(1).Subtract(Get1Microsecond());
    }

    public static DateTime RoundDownToStartOfWeek(
        this DateTime dateTime)
    {
        while (dateTime.DayOfWeek != DayOfWeek.Sunday)
            dateTime = dateTime.AddDays(-1);

        return dateTime.RoundDownToStartOfDay();
    }

    public static DateTime RoundUpToEndOfWeek(
        this DateTime dateTime)
    {
        while (dateTime.DayOfWeek != DayOfWeek.Saturday)
            dateTime = dateTime.AddDays(1);

        return dateTime.RoundUpToEndOfDay();
    }

    public static DateTime RoundDownToStartOfMonth(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1).RoundDownToStartOfDay();
    }

    public static DateTime RoundUpToEndOfMonth(
        this DateTime dateTime)
    {
        if (dateTime.Month == 12)
            return dateTime.RoundUpToEndOfYear();
        else
            return new DateTime(dateTime.Year, dateTime.Month + 1, 1).AddDays(-1).RoundUpToEndOfDay();
    }

    public static int GetStandardFiscalQuarter(
        this DateTime dateTime)
    {
        if (dateTime.Month <= 3)
            return 1;
        if (dateTime.Month <= 6)
            return 2;
        if (dateTime.Month <= 9)
            return 3;
        return 4;
    }

    public static DateTime RoundDownToStartOfStandardFiscalQuarter(
        this DateTime dateTime)
    {
        if (dateTime.Month <= 3)
            return new DateTime(dateTime.Year, 1, 1);
        if (dateTime.Month <= 6)
            return new DateTime(dateTime.Year, 4, 1);
        if (dateTime.Month <= 9)
            return new DateTime(dateTime.Year, 7, 1);
        return new DateTime(dateTime.Year, 10, 1);
    }

    public static DateTime RoundUpToEndOfStandardFiscalQuarter(
        this DateTime dateTime)
    {
        if (dateTime.Month <= 3)
            return new DateTime(dateTime.Year, 4, 1).AddDays(-1);
        if (dateTime.Month <= 6)
            return new DateTime(dateTime.Year, 7, 1).AddDays(-1);
        if (dateTime.Month <= 9)
            return new DateTime(dateTime.Year, 10, 1).AddDays(-1);
        return new DateTime(dateTime.Year + 1, 1, 1).AddDays(-1);
    }

    public static DateTime RoundDownToStartOfYear(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1).RoundDownToStartOfDay();
    }

    public static DateTime RoundUpToEndOfYear(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31).RoundUpToEndOfDay();
    }

    public static DateTime RoundUpToEndOfQuarterHour(
        this DateTime dateTime)
    {
        var minutes = 0;
        if (dateTime.Minute > 45 && dateTime.Minute < 59)
            minutes = 60;
        else if (dateTime.Minute > 30 && dateTime.Minute <= 45)
            minutes = 45;
        else if (dateTime.Minute > 15 && dateTime.Minute <= 30)
            minutes = 30;
        else if (dateTime.Minute > 0 && dateTime.Minute <= 15)
            minutes = 15;

        return dateTime.RoundDownToStartOfHour().AddMinutes(minutes);
    }

    public static DateTime RoundUpToDayOfWeek(
            this DateTime dateTime,
            DayOfWeek dayOfWeek)
    {
        var value = dateTime;
        while (value.DayOfWeek != dayOfWeek)
            value = value.AddDays(1);

        return value;
    }

    public static DateTime RoundDownToDayOfWeek(
        this DateTime dateTime,
        DayOfWeek dayOfWeek)
    {
        var value = dateTime;
        while (value.DayOfWeek != dayOfWeek)
            value = value.AddDays(-1);

        return value;
    }

    public static DateTime RoundToClosestDayOfWeek(
        this DateTime dateTime,
        DayOfWeek dayOfWeek)
    {
        if (dateTime.DayOfWeek == dayOfWeek)
        {
            return dateTime;
        }
        else
        {
            // Move to halfway down the week.
            var startDate = dateTime.AddDays(-3);
            for (int i = 0; i < 7; i++)
            {
                if (startDate.AddDays(i).DayOfWeek == dayOfWeek)
                    return dateTime;
            }
        }

        return dateTime;
    }

    public static string ToFileSystemSafeDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public static string ToFileSystemSafeDateTimeString(
        this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd_HH-mm-ss");
    }

    public static string ToAbbrevString(
        this DayOfWeek dayOfWeek)
    {
        return DateTime.Now.RoundUpToDayOfWeek(dayOfWeek).ToString("ddd");
    }


    public static string ToDayOfWeekDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString("dddd, MMM d");
    }

    public static long ToUnixEpochSeconds(
        this DateTime dateTimeUtc)
    {
        // Ensure the DateTime is in UTC.
        if (dateTimeUtc.Kind != DateTimeKind.Utc)
        {
            dateTimeUtc = dateTimeUtc.ToUniversalTime();
        }

        return (long)(dateTimeUtc - DateTime.UnixEpoch).TotalSeconds;
    }

    private static TimeSpan Get1Microsecond()
    {
        return new TimeSpan(0, 0, 0, 0, 0, 1);
    }
}
