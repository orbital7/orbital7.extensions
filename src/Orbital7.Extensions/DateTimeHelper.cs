namespace Orbital7.Extensions;

public static class DateTimeHelper
{
    public const string DEFAULT_DATE_FORMAT = "MM/dd/yyyy";

    public enum Holiday
    {
        NewYearsDay,
        MartinLutherKingJrDay,
        PresidentsDay,
        GoodFriday,
        Easter,
        MemorialDay,
        Juneteenth,
        IndependenceDay,
        LaborDay,
        ColumbusDay,
        VeteransDay,
        ThanksgivingDay,
        ChristmasDay,
    }

    public static TimeZoneInfo GetTimeZone(
        string timeZoneId)
    {
        if (!string.IsNullOrEmpty(timeZoneId))
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        else
            return TimeZoneInfo.Utc;
    }

    public static List<DateOnly> GetFederalHolidayDates(
        int year)
    {
        var holidays = new List<DateOnly>();

        holidays.Add(GetHolidayDate(Holiday.NewYearsDay, year));
        holidays.Add(GetHolidayDate(Holiday.MartinLutherKingJrDay, year));
        holidays.Add(GetHolidayDate(Holiday.PresidentsDay, year));
        holidays.Add(GetHolidayDate(Holiday.MemorialDay, year));

        if (year >= 2021)
            holidays.Add(GetHolidayDate(Holiday.Juneteenth, year));

        holidays.Add(GetHolidayDate(Holiday.IndependenceDay, year));
        holidays.Add(GetHolidayDate(Holiday.LaborDay, year));
        holidays.Add(GetHolidayDate(Holiday.ColumbusDay, year));
        holidays.Add(GetHolidayDate(Holiday.VeteransDay, year));
        holidays.Add(GetHolidayDate(Holiday.ThanksgivingDay, year));
        holidays.Add(GetHolidayDate(Holiday.ChristmasDay, year));

        return holidays;
    }

    public static DateOnly GetHolidayDate(
        Holiday holiday,
        int year)
    {
        switch (holiday)
        {
            // New Year's Day - January 1
            case Holiday.NewYearsDay:
                return AdjustForWeekend(new DateOnly(year, 1, 1));

            // Martin Luther King Jr. Day - 3rd Monday in January
            case Holiday.MartinLutherKingJrDay:
                return GetNthDayOfWeekInMonth(year, 1, DayOfWeek.Monday, 3);

            // Presidents' Day - 3rd Monday in February
            case Holiday.PresidentsDay:
                return GetNthDayOfWeekInMonth(year, 2, DayOfWeek.Monday, 3);

            // Good Friday - 2 days before Easter
            case Holiday.GoodFriday:
                return GetEasterSundayDate(year).AddDays(-2);

            // Easter - 1st Sunday after the first full moon on or after the vernal equinox
            case Holiday.Easter:
                return GetEasterSundayDate(year);

            // Presidents' Day - 3rd Monday in February
            case Holiday.MemorialDay:
                return GetLastDayOfWeekInMonth(year, 5, DayOfWeek.Monday);

            // Juneteenth - June 19 (observed since 2021)
            case Holiday.Juneteenth:
                return AdjustForWeekend(new DateOnly(year, 6, 19));

            // Independence Day - July 4
            case Holiday.IndependenceDay:
                return AdjustForWeekend(new DateOnly(year, 7, 4));

            // Labor Day - 1st Monday in September
            case Holiday.LaborDay:
                return GetNthDayOfWeekInMonth(year, 9, DayOfWeek.Monday, 1);

            // Columbus Day - 2nd Monday in October.
            case Holiday.ColumbusDay:
                return GetNthDayOfWeekInMonth(year, 10, DayOfWeek.Monday, 2);

            // Veterans Day - November 11
            case Holiday.VeteransDay:
                return AdjustForWeekend(new DateOnly(year, 11, 11));

            // Thanksgiving Day - 4th Thursday in November
            case Holiday.ThanksgivingDay:
                return GetNthDayOfWeekInMonth(year, 11, DayOfWeek.Thursday, 4);

            // Christmas Day - December 25
            case Holiday.ChristmasDay:
                return AdjustForWeekend(new DateOnly(year, 12, 25));

            default:
                throw new Exception($"Unrecognized holiday \"{holiday.ToDisplayString()}\"");
        }
    }

    public static DateOnly AdjustForWeekend(
        DateOnly holiday)
    {
        return holiday.DayOfWeek switch
        {
            DayOfWeek.Saturday => holiday.AddDays(-1), // Observed on Friday
            DayOfWeek.Sunday => holiday.AddDays(1),    // Observed on Monday
            _ => holiday                               // No adjustment needed
        };
    }

    public static DateOnly GetNthDayOfWeekInMonth(
        int year, 
        int month, 
        DayOfWeek dayOfWeek, 
        int occurrence)
    {
        // Find the first specified day of the week in the month
        var firstDay = new DateOnly(year, month, 1);
        var firstDayOfWeek = firstDay.DayOfWeek;

        int daysToAdd = ((int)dayOfWeek - (int)firstDayOfWeek + 7) % 7;
        var firstOccurrence = firstDay.AddDays(daysToAdd);

        // Add 7 days for each additional occurrence
        return firstOccurrence.AddDays(7 * (occurrence - 1));
    }

    public static DateOnly GetLastDayOfWeekInMonth(
        int year, 
        int month, 
        DayOfWeek dayOfWeek)
    {
        var lastDay = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
        var daysToSubtract = (7 + (int)lastDay.DayOfWeek - (int)dayOfWeek) % 7;

        return lastDay.AddDays(-daysToSubtract);
    }

    private static DateOnly GetEasterSundayDate(
        int year)
    {
        // Meeus/Jones/Butcher Gregorian algorithm.
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateOnly(year, month, day);
    }
}