using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orbital7.Extensions.Tests;

public class JsonSerializationHelperTests
{
    [Fact]
    public void DynamicSerialization()
    {
        var testObject = new
        {
            PropertyA = "Test A",
            PropertyB = 123,
            PropertyC = true,
        };

        var serialized = JsonSerializationHelper.SerializeToJson(
            testObject);

        Assert.NotNull(serialized);
        Assert.Contains($"{nameof(testObject.PropertyA).EncloseInQuotes()}:", serialized);
        Assert.Contains(testObject.PropertyA.EncloseInQuotes(), serialized);
        Assert.Contains($"{nameof(testObject.PropertyB).EncloseInQuotes()}:", serialized);
        Assert.Contains(testObject.PropertyB.ToString(), serialized);
        Assert.Contains($"{nameof(testObject.PropertyC).EncloseInQuotes()}:", serialized);
        Assert.Contains(testObject.PropertyC.ToString().ToLower(), serialized);
    }

    [Fact]
    public void ValueTupleSerialization()
    {
        var list = new List<(int, string)>()
        {
            new(0, "A"),
            new(1, "B"),
            new(2, "C"),
            new(3, "D"),
        };

        var serializedList = JsonSerializationHelper.SerializeToJson(list);

        var deserializedList = JsonSerializationHelper.DeserializeFromJson<List<(int i, string s)>>(serializedList);
        Assert.NotNull(deserializedList);
        Assert.Equal(list.Count, deserializedList.Count);

        for (int i = 0; i < list.Count; i++)
        {
            Assert.Equal(list[i].Item1, deserializedList[i].Item1);
            Assert.Equal(list[i].Item2, deserializedList[i].Item2);
        }
    }

    [Fact]
    public void TupleSerialization()
    {
        var list = new List<Tuple<int, string>>()
        {
            new(0, "A"),
            new(1, "B"),
            new(2, "C"),
            new(3, "D"),
        };

        var serializedList = JsonSerializationHelper.SerializeToJson(list);

        var deserializedList = JsonSerializationHelper.DeserializeFromJson<List<Tuple<int, string>>>(serializedList);
        Assert.NotNull(deserializedList);
        Assert.Equal(list.Count, deserializedList.Count);

        for (int i = 0; i < list.Count; i++)
        {
            Assert.Equal(list[i].Item1, deserializedList[i].Item1);
            Assert.Equal(list[i].Item2, deserializedList[i].Item2);
        }
    }

    [Fact]
    public void EmptySerialization()
    {
        Tuple<int, string>? tuple = null;

        var serialized = JsonSerializationHelper.SerializeToJson(tuple);
        Assert.Null(serialized);

        var deserialized = JsonSerializationHelper.DeserializeFromJson<Tuple<int, string>>(serialized);
        Assert.Null(deserialized);
    }

    [Fact]
    public void NamedIdSerialization()
    {
        var namedId = new NamedId<Guid>(Guid.NewGuid(), "Test Value");
        var serialized = JsonSerializationHelper.SerializeToJson(namedId);
        var deserialized = JsonSerializationHelper.DeserializeFromJson<NamedId<Guid>>(serialized);

        Assert.NotNull(deserialized);
        Assert.Equal(namedId.Id, deserialized.Id);
        Assert.Equal(namedId.Name, deserialized.Name);
    }

    [Fact]
    public void EnumStringSerialization()
    {
        var testClass = new TestClass
        {
            PropertyA = "Test A",
            PropertyB = "Test B",
            PropertyC = TestEnum.ValueA,
            PropertyD = TestEnum.ValueB,
        };

        var serialized = JsonSerializationHelper.SerializeToJson(testClass);
        Assert.NotNull(serialized);
        Assert.Contains("\"PropertyC\":\"ValueA\"", serialized);
        Assert.Contains("\"PropertyD\":\"value_b\"", serialized);

        var deserialized = JsonSerializationHelper.DeserializeFromJson<TestClass>(serialized);
        Assert.NotNull(deserialized);
        Assert.Equal(testClass.PropertyA, deserialized.PropertyA);
        Assert.Equal(testClass.PropertyB, deserialized.PropertyB);
        Assert.Equal(testClass.PropertyC, deserialized.PropertyC);
        Assert.Equal(testClass.PropertyD, deserialized.PropertyD);
    }

    [Theory]
    // ValueA has no specified JsonStringEnumMemberName -> any case is allowed.
    [InlineData("ValueA", "ValueA", false)]
    [InlineData("ValueA", "Valuea", false)]
    [InlineData("ValueA", "valueA", false)]
    [InlineData("ValueA", "valuea", false)]
    [InlineData("ValueA", "VALUEa", false)]
    [InlineData("ValueA", "VALUEA", false)]
    [InlineData("ValueA", "value_a", true)]

    // ValueB has a specified JsonStringEnumMemberName -> exact case is required.
    [InlineData("ValueB", "value_b", false)]
    [InlineData("ValueB", "Value_B", true)]
    [InlineData("ValueB", "Value_b", true)]
    [InlineData("ValueB", "value_B", true)]
    [InlineData("ValueB", "VALUE_B", true)]
    [InlineData("ValueB", "ValueB", true)]
    [InlineData("ValueB", "valueb", true)]
    public void EnumStringDeserialization_Casesensitivity(
        string expectedEnum,
        string serializedEnum,
        bool throwsJsonException)
    {
        var serializedEnumJson = serializedEnum.EncloseInQuotes();

        if (throwsJsonException)
        {
            Assert.Throws<JsonException>(() =>
                JsonSerializationHelper.DeserializeFromJson<TestEnum>(serializedEnumJson));
        }
        else
        {
            Assert.Equal(
                Enum.Parse<TestEnum>(expectedEnum, ignoreCase: true),
                JsonSerializationHelper.DeserializeFromJson<TestEnum>(serializedEnumJson));
        }
    }

    [Fact]
    public void ValueListDeserialization()
    {
        var testRecord = new TestRecord2
        {
            PropertyA = "Test A",
            PropertyB = new ValueList<string>(new[] { "Value1", "Value2" }),
            PropertyC = new ValueList<TestRecord3>(new[]
            {
                new TestRecord3 { PropertyA = "Nested A1", PropertyB = "Nested B1" },
                new TestRecord3 { PropertyA = "Nested A2", PropertyB = null },
            })
        };

        var serialized = JsonSerializationHelper.SerializeToJson(testRecord);
        var deserialized = JsonSerializationHelper.DeserializeFromJson<TestRecord2>(serialized);

        Assert.NotNull(serialized);
        Assert.NotNull(deserialized);
        Assert.Equal(testRecord.PropertyA, deserialized.PropertyA);
        Assert.Equal(testRecord.PropertyB, deserialized.PropertyB);
        Assert.Equal(testRecord.PropertyC, deserialized.PropertyC);
        Assert.Equal(testRecord, deserialized);
    }

    public class TestClass
    {
        public required string PropertyA { get; init; }

        public string? PropertyB { get; init; }

        public required TestEnum PropertyC { get; init; }

        public TestEnum? PropertyD { get; init; }
    }

    public enum TestEnum
    {
        ValueA,

        [JsonStringEnumMemberName("value_b")]
        ValueB,

        ValueC,
    }

    public record TestRecord1
    {
        public required string PropertyA { get; init; }

        public required ValueList<string> PropertyB { get; init; }
    }

    public record TestRecord2 :
        TestRecord1
    {
        public required ValueList<TestRecord3> PropertyC { get; init; }
    }

    public record TestRecord3
    {
        public required string PropertyA { get; init; }

        public string? PropertyB { get; init; }
    }
}
