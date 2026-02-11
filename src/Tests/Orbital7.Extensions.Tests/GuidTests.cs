namespace Orbital7.Extensions.Tests;

public class GuidTests
{
    [Fact]
    public void ShortGuidEncodeDecode()
    {
        var guid = GuidFactory.NextSequential();
        var encodedGuid = guid.ToShortString();
        Assert.NotNull(encodedGuid);
        Assert.Equal(GuidFactory.SHORT_GUID_LENGTH, encodedGuid.Length);

        var decodedGuid = GuidFactory.FromString(encodedGuid);
        Assert.Equal(guid, decodedGuid);

        Assert.Null(Guid.Empty.ToShortString());
        Assert.Null(((Guid?)null).ToShortString());
    }

    [Fact]
    public void EntitySequentialIdGeneration()
    {
        var entity1 = new TestGuidKeyedEntity();
        var entity2 = new TestGuidKeyedEntity();
        var entity3 = new TestGuidKeyedEntity();

        Assert.True(entity1.Id != Guid.Empty);
        Assert.True(entity2.Id != Guid.Empty);
        Assert.True(entity3.Id != Guid.Empty);

        Assert.True(entity1.Id != entity2.Id);
        Assert.True(entity2.Id != entity3.Id);
        Assert.True(entity1.Id != entity3.Id);

        Assert.True(entity1.Id < entity2.Id);
        Assert.True(entity2.Id < entity3.Id);
        Assert.True(entity1.Id < entity3.Id);

        var list = new List<TestGuidKeyedEntity> { entity3, entity1, entity2 };
        var sortedList = list.OrderBy(x => x.Id).ToList();
        Assert.Equal(entity1.Id, sortedList[0].Id);
        Assert.Equal(entity2.Id, sortedList[1].Id);
        Assert.Equal(entity3.Id, sortedList[2].Id);
    }

    public class TestGuidKeyedEntity : EntityGuidKeyedBase
    {

    }
}