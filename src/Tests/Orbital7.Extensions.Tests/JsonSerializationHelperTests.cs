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
        Tuple<int, string> tuple = null;

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

        Assert.Equal(namedId.Id, deserialized.Id);
        Assert.Equal(namedId.Name, deserialized.Name);
    }
}
