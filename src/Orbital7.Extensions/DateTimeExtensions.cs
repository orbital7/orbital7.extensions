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

    public static DateTime? UtcToTimeZone(
        this DateTime? dateTimeUtc, 
        TimeZoneInfo timeZone)
    {
        if (dateTimeUtc.HasValue)
        {
            return dateTimeUtc.Value.UtcToTimeZone(timeZone);
        }
        
        return null;
    }

    public static DateTime TimeZoneToUtc(
        this DateTime dateTimeInTimeZone,
        TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTime(dateTimeInTimeZone, timeZone, TimeZoneInfo.Utc);
    }

    public static DateTime? TimeZoneToUtc(
        this DateTime? dateTimeInTimeZone,
        TimeZoneInfo timeZone)
    {
        if (dateTimeInTimeZone.HasValue)
        {
            return dateTimeInTimeZone.Value.TimeZoneToUtc(timeZone);
        }

        return null;
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

    public static int CalculateMonthsDifference(
        this DateTime dateTime, 
        DateTime dateToCompare)
    {
        return ((dateTime.Year - dateToCompare.Year) * 12) + dateTime.Month - dateToCompare.Month;
    }

    public static int CalculateQuartersDifference(
        this DateTime dateTime, 
        DateTime dateToCompare)
    {
        return ((dateTime.Year - dateToCompare.Year) * 4) + dateTime.ToQuarter() - dateToCompare.ToQuarter();
    }

    public static double CalculateAverageMonthsDifference(
        this DateTime dateTime, 
        DateTime dateToCompare)
    {
        return dateTime.Subtract(dateToCompare).Days / (365.25 / 12);
    }

    public static string ToDefaultDateString(
        this DateOnly date)
    {
        return date.ToString(DateTimeHelper.DEFAULT_DATE_FORMAT);
    }

    public static string ToDefaultDateString(
        this DateOnly? date,
        string nullValue = null)
    {
        if (date.HasValue)
            return date.Value.ToDefaultDateString();
        else
            return nullValue;
    }

    public static string ToDefaultDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString(DateTimeHelper.DEFAULT_DATE_FORMAT);
    }

    public static string ToDefaultDateString(
        this DateTime? dateTime, 
        string nullValue = null)
    {
        if (dateTime.HasValue)
            return dateTime.Value.ToDefaultDateString();
        else
            return nullValue;
    }
    
    public static string ToDefaultTimeString(
        this DateTime dateTime)
    {
        // TODO: Not liking the am/pm without a space, and
        // the conditional seconds are a problem; go back
        // to default for now.
        //var fi = new DateTimeFormatInfo()
        //{
        //    AMDesignator = "am",
        //    PMDesignator = "pm"
        //};

        //var format = dateTime.Second == 0 ?
        //    "h:mmtt" :
        //    "h:mm:sstt";
        var format = "h:mm tt";

        return dateTime.ToString(format); //, fi);
    }

    public static string ToDefaultTimeString(
        this DateTime? dateTime, 
        string nullValue = null)
    {
        if (dateTime.HasValue)
            return dateTime.Value.ToDefaultTimeString();
        else
            return nullValue;
    }

    public static string ToDefaultDateTimeString(
        this DateTime dateTime)
    {
        return dateTime.ToDefaultDateString() + " " + dateTime.ToDefaultTimeString();
    }

    public static string ToDefaultDateTimeString(
        this DateTime? dateTime, 
        string nullValue = null)
    {
        if (dateTime.HasValue)
            return dateTime.Value.ToDefaultDateTimeString();
        else
            return nullValue;
    }

    public static string ToMonthDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString("MMMM yyyy");
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
        return string.Format("{0:yyyy-MM-dd}", dateTimeUtc);
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

    public static DateTime RoundToStartOfBusinessDay(
        this DateTime dateTime)
    {
        if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            dateTime = dateTime.AddDays(2);
        else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            dateTime = dateTime.AddDays(1);

        return dateTime.RoundToStartOfDay();
    }

    public static DateTime RoundToStartOfNextBusinessDay(
        this DateTime dateTime)
    {
        return dateTime.AddDays(1).RoundToStartOfBusinessDay();
    }

    public static DateTime RoundToStartOfMinute(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);
    }

    public static DateTime RoundToEndOfMinute(
        this DateTime dateTime)
    {
        return dateTime.RoundToStartOfMinute().AddMinutes(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime RoundToStartOfHour(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind);
    }

    public static DateTime RoundToEndOfHour(
        this DateTime dateTime)
    {
        return dateTime.RoundToStartOfHour().AddHours(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime RoundToStartOfDay(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static DateTime RoundToEndOfDay(
        this DateTime dateTime)
    {
        return dateTime.RoundToStartOfDay().AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime RoundToStartOfWeek(
        this DateTime dateTime)
    {
        while (dateTime.DayOfWeek != DayOfWeek.Sunday)
            dateTime = dateTime.AddDays(-1);

        return dateTime.RoundToStartOfDay();
    }

    public static DateTime RoundToEndOfWeek(
        this DateTime dateTime)
    {
        while (dateTime.DayOfWeek != DayOfWeek.Saturday)
            dateTime = dateTime.AddDays(1);

        return dateTime.RoundToEndOfDay();
    }

    public static DateTime RoundToStartOfMonth(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1).RoundToStartOfDay();
    }

    public static DateTime RoundToEndOfMonth(
        this DateTime dateTime)
    {
        if (dateTime.Month == 12)
            return dateTime.RoundToEndOfYear();
        else
            return new DateTime(dateTime.Year, dateTime.Month + 1, 1).AddDays(-1).RoundToEndOfDay();
    }

    public static int ToQuarter(
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

    public static DateTime RoundToStartOfQuarter(
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

    public static DateTime RoundToEndOfQuarter(
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

    public static DateTime RoundToStartOfYear(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1).RoundToStartOfDay();
    }

    public static DateTime RoundToEndOfYear(
        this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31).RoundToEndOfDay();
    }

    public static DateTime RoundToEndOfQuarterHour(
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

        return dateTime.RoundToStartOfHour().AddMinutes(minutes);
    }

    public static DateTime RoundForwardToDayOfWeek(
            this DateTime dateTime,
            DayOfWeek dayOfWeek)
    {
        var value = dateTime;
        while (value.DayOfWeek != dayOfWeek)
            value = value.AddDays(1);

        return value;
    }

    public static DateTime RoundBackwardToDayOfWeek(
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
        return DateTime.Now.RoundForwardToDayOfWeek(dayOfWeek).ToString("ddd");
    }


    public static string ToDayOfWeekDateString(
        this DateTime dateTime)
    {
        return dateTime.ToString("dddd, MMM d");
    }

    public static string ToDayOfWeekDateString(
        this DateTime? dateTime,
        string nullValue = null)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.ToDayOfWeekDateString();
        }
        else
        {
            return nullValue;
        }
    }
}
