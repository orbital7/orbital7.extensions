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
    [InlineData(5.65, 0.25, 5.75)]
    [InlineData(5.51, 0.25, 5.75)]
    [InlineData(5.5, 0.25, 5.5)]
    [InlineData(6, 0.25, 6)]
    [InlineData(5.65, -0.25, 5.5)]
    [InlineData(5.51, -0.25, 5.5)]
    [InlineData(5.5, -0.25, 5.5)]
    [InlineData(6, -0.25, 6)]
    [InlineData(-5.65, 0.25, -5.5)]
    [InlineData(-5.51, 0.25, -5.5)]
    [InlineData(-5.5, 0.25, -5.5)]
    [InlineData(-6, 0.25, -6)]
    [InlineData(5, 0, 5)]
    [InlineData(0, 0.1, 0)]
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
    [InlineData(5.65, 0.25, 5.5)]
    [InlineData(5.51, 0.25, 5.5)]
    [InlineData(5.5, 0.25, 5.5)]
    [InlineData(6, 0.25, 6)]
    [InlineData(-5.65, 0.25, -5.75)]
    [InlineData(-5.51, 0.25, -5.75)]
    [InlineData(-5.5, 0.25, -5.5)]
    [InlineData(-6, 0.25, -6)]
    [InlineData(5, 0, 5)]
    [InlineData(0, 0.1, 0)]
    public void RoundDownToNearestStep(
        decimal value,
        decimal step,
        decimal expected)
    {
        Assert.Equal(expected, value.RoundDownToNearestStep(step));
    }

    [Theory]
    [InlineData(5.65, 5.75)]
    [InlineData(5.51, 5.75)]
    [InlineData(5.5, 5.5)]
    [InlineData(6, 6)]
    public void RoundUpToLargestQuarter(
        decimal value,
        decimal expected)
    {
        Assert.Equal(expected, value.RoundUpToLargestQuarter());
    }

    [Theory]
    [InlineData(5.65, 5.5)]
    [InlineData(5.51, 5.5)]
    [InlineData(5.5, 5.5)]
    [InlineData(6, 6)]
    public void RoundDownToSmallestQuarter(
        decimal value, 
        decimal expected)
    {
        Assert.Equal(expected, value.RoundDownToSmallestQuarter());
    }
}
