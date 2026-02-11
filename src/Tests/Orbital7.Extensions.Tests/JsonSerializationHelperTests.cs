using System.Text.Json.Serialization;

namespace Orbital7.Extensions.Tests;

public class JsonSerializationHelperTests
{
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
}
