namespace Orbital7.Extensions.Tests;

public class DateOnlyExtensionsTests
{
    [Theory]
    [InlineData("01/01/2023", false)]  // New Year's Day
    [InlineData("01/02/2023", false)]  // Monday after New Year's Day
    [InlineData("01/03/2023", true)]
    [InlineData("01/04/2023", true)]
    [InlineData("01/05/2023", true)]
    [InlineData("01/06/2023", true)]
    [InlineData("01/07/2023", false)]
    [InlineData("01/08/2023", false)]
    public void IsBusinessDay(
        string date,
        bool expected)
    {
        var dateOnly = DateOnly.Parse(date);
        var result = dateOnly.IsBusinessDay();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("05/25/2025", "06/25/2025", 1)]
    [InlineData("05/25/2025", "05/25/2026", 12)]
    public void CalculateMonthsDifference(
        string date1,
        string date2,
        int expectedMonths)
    {
        var dateOnly1 = ParsingHelper.ParseDateOnly(date1);
        var dateOnly2 = ParsingHelper.ParseDateOnly(date2);

        var months = dateOnly2.CalculateMonthsDifference(dateOnly1);
        var roundedMonths = Math.Round(months, 0);

        Assert.Equal(expectedMonths, roundedMonths);
    }
}
