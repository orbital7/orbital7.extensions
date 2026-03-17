namespace Orbital7.Extensions.Tests;

public class ValueListTests
{
    [Fact]
    public void ValueList_Equality()
    {
        var list1 = new ValueList<int>(new[] { 1, 2, 3 });
        var list2 = new ValueList<int>(new[] { 1, 2, 3 });
        var list3 = new ValueList<int>(new[] { 3, 2, 1 });
        Assert.Equal(list1, list2);
        Assert.NotEqual(list1, list3);
    }

    [Fact]
    public void ValueList_RecordEquality()
    {
        var record1 = new TestRecord { Name = "Alice", Numbers = new ValueList<int>(new[] { 1, 2, 3 }) };
        var record2 = new TestRecord { Name = "Alice", Numbers = new ValueList<int>(new[] { 1, 2, 3 }) };
        var record3 = new TestRecord { Name = "John", Numbers = new ValueList<int>(new[] { 1, 2, 3 }) };
        var record4 = new TestRecord { Name = "Alice", Numbers = new ValueList<int>(new[] { 3, 2, 1 }) };
        Assert.Equal(record1, record2);
        Assert.NotEqual(record1, record3);
        Assert.NotEqual(record1, record4);
    }

    public record TestRecord
    {
        public required string Name { get; init; }

        public required ValueList<int> Numbers { get; init; }
    }
}
