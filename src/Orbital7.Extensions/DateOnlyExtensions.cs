namespace Orbital7.Extensions;

public static class DateOnlyExtensions
{
    public static string ToDefaultDateString(
        this DateOnly date)
    {
        return date.ToString(DateTimeHelper.DEFAULT_DATE_FORMAT);
    }

    public static string? ToDefaultDateString(
        this DateOnly? date,
        string? nullValue = null)
    {
        if (date.HasValue)
            return date.Value.ToDefaultDateString();
        else
            return nullValue;
    }

    public static string ToMonthDateString(
        this DateOnly date)
    {
        return date.ToString("MMMM yyyy");
    }

    public static DateOnly AddBusinessDays(
        this DateOnly date,
        int days,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        var totalDays = Math.Abs(days);
        var delta = days < 0 ? -1 : 1;
        var value = date;

        int businessDays = 0;
        while (businessDays < totalDays)
        {
            value = value.AddDays(delta);
            if (value.IsBusinessDay(nonBusinessDaysOfWeekOverride, holidayDatesOverride))
            {
                businessDays++;
            }
        }

        return value;
    }

    public static DateOnly RoundUpToBusinessDay(
        this DateOnly date,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        var value = date;
        while (!value.IsBusinessDay(nonBusinessDaysOfWeekOverride, holidayDatesOverride))
        {
            value = value.AddDays(1);
        }
        return value;
    }

    public static DateOnly RoundDownToBusinessDay(
        this DateOnly date,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        var value = date;
        while (!value.IsBusinessDay(nonBusinessDaysOfWeekOverride, holidayDatesOverride))
        {
            value = value.AddDays(-1);
        }
        return value;
    }

    public static DateOnly RoundUpToNextBusinessDay(
        this DateOnly date,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        return date
            .AddDays(1)
            .RoundUpToBusinessDay(
                nonBusinessDaysOfWeekOverride, 
                holidayDatesOverride);
    }

    public static DateOnly RoundDownToPreviousBusinessDay(
        this DateOnly date,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        return date
            .AddDays(-1)
            .RoundDownToBusinessDay(
                nonBusinessDaysOfWeekOverride, 
                holidayDatesOverride);
    }

    public static DateOnly MinimallyShiftUpOrDownToBusinessDay(
        this DateOnly date,
        int daysToShift,
        bool shiftUpIfEqual,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        var dateShiftedUp = date.AddBusinessDays(
            Math.Abs(daysToShift),
            nonBusinessDaysOfWeekOverride,
            holidayDatesOverride);

        var dateShiftedDown = date.AddBusinessDays(
            -1 * Math.Abs(daysToShift),
            nonBusinessDaysOfWeekOverride,
            holidayDatesOverride);

        var daysShiftedUp = dateShiftedUp.DayNumber - date.DayNumber;
        var daysShiftedDown = date.DayNumber - dateShiftedDown.DayNumber;

        if (daysShiftedUp < daysShiftedDown ||
            (daysShiftedUp == daysShiftedDown && shiftUpIfEqual))
        {
            return dateShiftedUp;
        }
        else
        {
            return dateShiftedDown;
        }
    }

    public static bool IsBusinessDay(
        this DateOnly date,
        IList<DayOfWeek>? nonBusinessDaysOfWeekOverride = null,
        IList<DateOnly>? holidayDatesOverride = null)
    {
        // Ensure we have values.
        holidayDatesOverride ??= DateTimeHelper.GetFederalHolidayDates(date.Year);
        nonBusinessDaysOfWeekOverride ??= new List<DayOfWeek>
        {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        return !nonBusinessDaysOfWeekOverride.Contains(date.DayOfWeek) &&
               !holidayDatesOverride.Contains(date);
    }

    public static DateOnly RoundUpToDayOfWeek(
        this DateOnly date,
        DayOfWeek dayOfWeek)
    {
        var value = date;
        while (value.DayOfWeek != dayOfWeek)
        {
            value = value.AddDays(1);
        }
        return value;
    }

    public static DateOnly RoundDownToDayOfWeek(
        this DateOnly date,
        DayOfWeek dayOfWeek)
    {
        var value = date;
        while (value.DayOfWeek != dayOfWeek)
        {
            value = value.AddDays(-1);
        }
        return value;
    }

    public static DateOnly RoundToClosestDayOfWeek(
        this DateOnly date,
        DayOfWeek dayOfWeek)
    {
        if (date.DayOfWeek == dayOfWeek)
        {
            return date;
        }
        else
        {
            // Move to halfway down the week.
            var startDate = date.AddDays(-3);
            for (int i = 0; i < 7; i++)
            {
                if (startDate.AddDays(i).DayOfWeek == dayOfWeek)
                    return date;
            }
        }

        return date;
    }

    public static DateOnly RoundToNthDayOfWeekOfMonth(
        this DateOnly date,
        DayOfWeek dayOfWeek,
        int occurance)
    {
        return DateTimeHelper.GetNthDayOfWeekInMonth(
            date.Year, 
            date.Month, 
            dayOfWeek, 
            occurance);
    }

    public static DateOnly RoundDownToStartOfMonth(
        this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly RoundUpToEndOfMonth(
        this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static int GetStandardFiscalQuarter(
        this DateOnly date)
    {
        if (date.Month <= 3)
            return 1;
        if (date.Month <= 6)
            return 2;
        if (date.Month <= 9)
            return 3;
        return 4;
    }

    public static DateOnly RoundDownToStartOfStandardFiscalQuarter(
        this DateOnly date)
    {
        if (date.Month <= 3)
            return new DateOnly(date.Year, 1, 1);
        if (date.Month <= 6)
            return new DateOnly(date.Year, 4, 1);
        if (date.Month <= 9)
            return new DateOnly(date.Year, 7, 1);
        return new DateOnly(date.Year, 10, 1);
    }

    public static DateOnly RoundUpToEndOfStandardFiscalQuarter(
        this DateOnly date)
    {
        if (date.Month <= 3)
            return new DateOnly(date.Year, 4, 1).AddDays(-1);
        if (date.Month <= 6)
            return new DateOnly(date.Year, 7, 1).AddDays(-1);
        if (date.Month <= 9)
            return new DateOnly(date.Year, 10, 1).AddDays(-1);
        return new DateOnly(date.Year + 1, 1, 1).AddDays(-1);
    }

    public static DateOnly RoundDownToStartOfYear(
        this DateOnly date)
    {
        return new DateOnly(date.Year, 1, 1);
    }

    public static DateOnly RoundUpToEndOfYear(
        this DateOnly date)
    {
        return new DateOnly(date.Year, 12, 31);
    }

    public static string ToFileSystemSafeDateString(
        this DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}
