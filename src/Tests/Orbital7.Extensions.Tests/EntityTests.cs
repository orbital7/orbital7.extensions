namespace Orbital7.Extensions.Tests;

public class EntityTests
{
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