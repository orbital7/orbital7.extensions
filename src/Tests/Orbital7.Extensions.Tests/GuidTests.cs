namespace Orbital7.Extensions.Tests;

public class GuidTests
{
    [Fact]
    public void ShortGuidEncodeDecode()
    {
        var guid = GuidFactory.NextSequential();
        var encodedGuid = guid.ToShortString();
        var decodedGuid = GuidFactory.FromString(encodedGuid);

        Assert.NotNull(encodedGuid);
        Assert.Equal(GuidFactory.SHORT_GUID_LENGTH, encodedGuid.Length);
        Assert.Equal(guid, decodedGuid);
        Assert.Null(Guid.Empty.ToShortString());
        Assert.Null(((Guid?)null).ToShortString());
    }

    [Fact]
    public void EntitySequentialIdGeneration()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        var entity3 = new TestEntity();

        Assert.True(entity1.Id != Guid.Empty);
        Assert.True(entity2.Id != Guid.Empty);
        Assert.True(entity3.Id != Guid.Empty);

        Assert.True(entity1.Id != entity2.Id);
        Assert.True(entity2.Id != entity3.Id);
        Assert.True(entity1.Id != entity3.Id);

        Assert.True(entity1.Id < entity2.Id);
        Assert.True(entity2.Id < entity3.Id);
        Assert.True(entity1.Id < entity3.Id);
    }

    public class TestEntity : EntityBase
    {

    }
}