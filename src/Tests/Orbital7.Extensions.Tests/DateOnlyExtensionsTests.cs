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
    public void TestIsBusinessDay(
        string date,
        bool expected)
    {
        var dateOnly = DateOnly.Parse(date);
        var result = dateOnly.IsBusinessDay();
        Assert.Equal(expected, result);
    }
}
