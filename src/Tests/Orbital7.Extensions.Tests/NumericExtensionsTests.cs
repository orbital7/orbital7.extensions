namespace Orbital7.Extensions.Tests;

public class NumericExtensionsTests
{
    [Theory]
    [InlineData(508, 5, 510)]
    [InlineData(510, 5, 510)]
    [InlineData(513, 5, 515)]
    [InlineData(515, 5, 515)]
    [InlineData(508, 10, 510)]
    [InlineData(510, 10, 510)]
    [InlineData(513, 10, 520)]
    [InlineData(515, 10, 520)]
    [InlineData(518, 10, 520)]
    [InlineData(520, 10, 520)]
    public void RoundUpToNearestStep(
        decimal value,
        decimal step,
        decimal expected)
    {
        Assert.Equal(expected, value.RoundUpToNearestStep(step));
    }

    [Theory]
    [InlineData(508, 5, 505)]
    [InlineData(510, 5, 510)]
    [InlineData(513, 5, 510)]
    [InlineData(515, 5, 515)]
    [InlineData(508, 10, 500)]
    [InlineData(510, 10, 510)]
    [InlineData(513, 10, 510)]
    [InlineData(515, 10, 510)]
    [InlineData(518, 10, 510)]
    [InlineData(520, 10, 520)]
    public void RoundDownToNearestStep(
        decimal value,
        decimal step,
        decimal expected)
    {
        Assert.Equal(expected, value.RoundDownToNearestStep(step));
    }
}
