namespace Orbital7.Extensions.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public void CloneIgnoringReferenceProperties_WithRecord()
    {
        var original = new TestRecord1
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithRequiredRecord()
    {
        var original = new TestRecord2
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithPropertyOverride()
    {
        string overrideValue = "ValueOverride";

        var original = new TestRecord1
        {
            Name = "Test",
            Value = "Value"
        };

        var clone = original.CloneIgnoringReferenceProperties(new Dictionary<string, object?>
        {
            { nameof(TestRecord1.Value), overrideValue }
        });

        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(overrideValue, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithClass()
    {
        var original = new TestClass1
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithRecordTypeTransformation()
    {
        var original = new TestRecord1
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties<TestRecord1, TestRecord2>();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithRequiredClass()
    {
        var original = new TestClass2
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithClassTypeTransformation()
    {
        var original = new TestClass1
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties<TestClass1, TestClass2>();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithRecordToClassTypeTransformation()
    {
        var original = new TestRecord2
        {
            Name = "Test",
            Value = "Value"
        };
        var clone = original.CloneIgnoringReferenceProperties<TestRecord2, TestClass2>();
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Value, clone.Value);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithArray()
    {
        NamedValue<USState>[] original =
        [
            new NamedValue<USState>("Alabama", USState.AL),
            new NamedValue<USState>("Alaska", USState.AK),
            new NamedValue<USState>("Arizona", USState.AZ),
        ];

        var clone = original.CloneIgnoringReferenceProperties();

        Assert.NotSame(original, clone);
        Assert.Equal(original.Length, clone.Length);
    }

    [Fact]
    public void CloneIgnoringReferenceProperties_WithList()
    {
        var original = new List<NamedValue<USState>>()
        {
            new NamedValue<USState>("Alabama", USState.AL),
            new NamedValue<USState>("Alaska", USState.AK),
            new NamedValue<USState>("Arizona", USState.AZ),
        };

        var clone = original.CloneIgnoringReferenceProperties();

        Assert.NotSame(original, clone);
        Assert.Equal(original.Count, clone.Count);
    }

    public class TestRecord1
    {
        public string? Name { get; init; }
        public string? Value { get; init; }
    }

    public class TestRecord2
    {
        public required string Name { get; init; }
        public required string Value { get; init; }
    }

    public class TestClass1
    {
        public string? Name { get; init; }
        public string? Value { get; init; }
    }

    public class TestClass2
    {
        public required string Name { get; init; }
        public required string Value { get; init; }
    }
}
