namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class TimeOnlyConverter : 
    ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan))
    { }
}
