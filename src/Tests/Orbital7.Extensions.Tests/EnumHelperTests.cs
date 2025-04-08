namespace Orbital7.Extensions.Tests;

public class EnumHelperTests
{
    [Fact]
    public void ToValueList()
    {
        var list = EnumHelper.ToValueList<USState>();

        Assert.True(list.Count > 0);
    }
}
