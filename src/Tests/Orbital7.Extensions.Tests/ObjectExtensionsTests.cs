namespace Orbital7.Extensions.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public void CloneIgnoringReferenceProperties()
    {
        var list = new List<NamedValue<USState>>()
        {
            new NamedValue<USState>("Alabama", USState.AL),
            new NamedValue<USState>("Alaska", USState.AK),
            new NamedValue<USState>("Arizona", USState.AZ),
        };

        var clonedList = list.CloneIgnoringReferenceProperties();

        Assert.NotSame(list, clonedList);
        Assert.Equal(list.Count, clonedList.Count);
    }
}
