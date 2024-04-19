namespace Orbital7.Extensions.Tests;

public class EnumHelperTests
{
    [Fact]
    public void ToValueList()
    {
        var list1 = EnumHelper.ToValueList<USState>();
        var list2 = EnumHelper.ToValueList<USState?>();

        Assert.True(list1.Count > 0);
        Assert.Equal(list1.Count, list2.Count);
    }
}
