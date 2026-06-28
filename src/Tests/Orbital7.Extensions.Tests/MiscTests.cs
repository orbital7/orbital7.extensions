namespace Orbital7.Extensions.Tests;

public class MiscTests
{
    [Fact]
    public void EqualityOperatorWithNullableDecimals()
    {
        decimal a = 0;
        decimal? b = 0;
        decimal? c = null;

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.False(b == c);
        Assert.True(c == null);
        Assert.False(c == 0);

        Assert.True(b != c);
        Assert.True(c != 0);
    }
}
