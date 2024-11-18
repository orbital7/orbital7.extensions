namespace System;

public class TimeConverter
{
    private TimeZoneInfo _customLocalTimeZone;

    public event EventHandler LocalTimeZoneChanged;

    public bool IsCustomLocalTimeZoneSet => _customLocalTimeZone != null;

    public TimeZoneInfo LocalTimeZone => _customLocalTimeZone ?? TimeZoneInfo.Local;

    public virtual void SetCustomLocalTimeZone(
        string timeZoneId)
    {
        TimeZoneInfo timeZone = null;

        if (!TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out timeZone))
        {
            timeZone = null;
        }

        if (timeZone != TimeZoneInfo.Local)
        {
            _customLocalTimeZone = timeZone;
            this.LocalTimeZoneChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public virtual DateTime ToDateTime(
        DateTime dateTime,
        string timeZoneId)
    {
        return dateTime.Kind switch
        {
            //DateTimeKind.Unspecified => throw new InvalidOperationException("Unable to convert unspecified DateTime to local time"),
            DateTimeKind.Local => dateTime,
            _ => DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)), DateTimeKind.Local),
        };
    }

    public virtual DateTime ToDateTime(
        DateTimeOffset dateTimeOffset,
        string timeZoneId)
    {
        var local = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
        local = DateTime.SpecifyKind(local, DateTimeKind.Local);
        return local;
    }

    public virtual DateTime ToLocalDateTime(
        DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            //DateTimeKind.Unspecified => throw new InvalidOperationException("Unable to convert unspecified DateTime to local time"),
            DateTimeKind.Local => dateTime,
            _ => DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(dateTime, this.LocalTimeZone), DateTimeKind.Local),
        };
    }

    public virtual DateTime ToLocalDateTime(
        DateTimeOffset dateTimeOffset)
    {
        var local = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, this.LocalTimeZone);
        local = DateTime.SpecifyKind(local, DateTimeKind.Local);
        return local;
    }
}
