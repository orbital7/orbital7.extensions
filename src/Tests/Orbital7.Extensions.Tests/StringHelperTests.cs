namespace Orbital7.Extensions.Tests;

public class StringHelperTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("DoesNotExist", null)]
    [InlineData("OR", USState.OR)]
    public void ParseNullableEnum(
        string? value,
        USState? expected)
    {
        var actual = StringHelper.ParseNullableEnum<USState>(value);
        Assert.Equal(expected, actual);
    }
}
