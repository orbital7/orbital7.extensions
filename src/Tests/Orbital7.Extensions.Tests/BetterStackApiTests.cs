using Microsoft.Extensions.Configuration;
using Orbital7.Extensions.Apis.BetterStack;
using System.Text.Json.Serialization;

namespace Orbital7.Extensions.Tests;

public class BetterStackApiTests
{
    public string BetterStackUptimeApiToken { get; private set; }

    public string BetterStackLogsSourceToken { get; private set; }

    public BetterStackApiTests()
    {
        var config = ConfigurationHelper.GetConfigurationWithUserSecrets<BetterStackApiTests>();
        this.BetterStackUptimeApiToken = config["BetterStackUptimeApiToken"];
        this.BetterStackLogsSourceToken = config["BetterStackLogsSourceToken"];
    }

    [SkippableFact]
    public async Task TestUptimeHeartbeatsApi()
    {
        // NOTE: This test is not very clean; we try all CRUD operations, but
        // there's no conditional clean up. This test was used moreso for 
        // internal testing than for regular testing.

        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.BetterStackUptimeApiToken.HasText());

        // Create the client and service.
        var client = new BetterStackClient(this.BetterStackUptimeApiToken);
        var heartbeatsService = new UptimeHeartbeatsService(client);

        // Create a heartbeat.
        const string CREATE_HEARTBEAT_NAME = "Test Heartbeat";
        var createHeartbeatResponse = await heartbeatsService.CreateAsync(
            new HeartbeatRequest()
            {
                Name = CREATE_HEARTBEAT_NAME,
                Period = 180,
                Grace = 36,
                Email = true,
                Paused = true,
            });
        Assert.True(createHeartbeatResponse != null);

        // Validate the heartbeat creation.
        var heartbeat = createHeartbeatResponse.Data;
        Assert.True(heartbeat != null);
        Assert.True(heartbeat.Attributes != null);
        Assert.True(heartbeat.Id.HasText());
        Assert.Equal(CREATE_HEARTBEAT_NAME, heartbeat.Attributes.Name);
        Assert.True(heartbeat.Attributes.Paused);

        // Test listing heartbeats.
        var heartbeatsResponse = await heartbeatsService.ListAllExistingAsync();
        Assert.NotNull(heartbeatsResponse.Data.Where(x => x.Id == heartbeat.Id).FirstOrDefault());

        // Update the heartbeat.
        const string UPDATE_HEARTBEAT_NAME = "Test Heartbeat (updated)";
        var updateHeartbeatResponse = await heartbeatsService.UpdateAsync(
            heartbeat.Id,
            new HeartbeatRequest()
            {
                Name = UPDATE_HEARTBEAT_NAME,
                Paused = false,
            });
        Assert.True(updateHeartbeatResponse != null);

        // Validate the heartbeat update.
        heartbeat = updateHeartbeatResponse.Data;
        Assert.True(heartbeat != null);
        Assert.True(heartbeat.Attributes != null);
        Assert.True(heartbeat.Id.HasText());
        Assert.Equal(UPDATE_HEARTBEAT_NAME, heartbeat.Attributes.Name);
        Assert.False(heartbeat.Attributes.Paused);

        // Delete the heartbeat.
        await heartbeatsService.DeleteAsync(heartbeat.Id);
        heartbeatsResponse = await heartbeatsService.ListAllExistingAsync();
        Assert.Null(heartbeatsResponse.Data.Where(x => x.Id == heartbeat.Id).FirstOrDefault());

        // Test parsing page index.
        var pageIndex = heartbeatsResponse.ParsePageIndex(heartbeatsResponse.Pagination.First);
        Assert.Equal(1, pageIndex);
    }

    [SkippableFact]
    public async Task TestLogsUpload()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.BetterStackLogsSourceToken.HasText());

        // Create the client and service.
        var client = new BetterStackClient(this.BetterStackLogsSourceToken);
        var logsUploadService = new LogsUploadService(client);

        // Create an event.
        var logEvent = new LogEvent()
        {
            Message = "Single-log-event from Orbital7.Extensions.Tests",
            Level = "Information",
            Metadata = new Dictionary<string, object>()
            {
                { 
                    "Test Data 1", 
                    CreateTestEntity1()
                },
                {
                    "Test Data 2",
                    "Simple string"
                },
                {
                    "Test Data 3",
                    JsonSerializationHelper.SerializeToJson(CreateTestEntity1())
                },
            }
        };

        // Upload single log event.
        await logsUploadService.LogEventAsync(logEvent);

        // Create multiple events.
        var logEvents = new LogEvent[]
        {
            new LogEvent()
            {
                Message = "Multi-log-event-1 from Orbital7.Extensions.Tests",
                Level = "Information",
                Metadata = new Dictionary<string, object>()
                {
                    {
                        "Test Data 1",
                        CreateTestEntity1()
                    },
                },
            },
            new LogEvent()
            {
                Message = "Multi-log-event-2 from Orbital7.Extensions.Tests",
                Level = "Information",
                Metadata = new Dictionary<string, object>()
                {
                    {
                        "Test Data 2",
                        CreateTestEntity1()
                    },
                },
            },
        };

        // Upload multiple log events.
        await logsUploadService.LogEventsAsync(logEvents);
    }

    private TestEntity1 CreateTestEntity1()
    {
        return new TestEntity1()
        {
            Name = Guid.NewGuid().ToString(),
            ChildEntity = new TestEntity2()
            {
                Id = Guid.NewGuid(),
                ChildEntities = new List<TestEntity3>()
                {
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = Guid.NewGuid().ToString(),
                        TestValue3 = true,
                        TestValue4 = DateTime.Now,
                    },
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = null,
                        TestValue3 = null,
                        TestValue4 = DateTime.Now,
                    },
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = Guid.NewGuid().ToString(),
                        TestValue3 = false,
                        TestValue4 = DateTime.UtcNow,
                    },
                            }
            }
        };
    }

    private class TestEntity1
    {
        public string Name { get; set; }

        public TestEntity2 ChildEntity { get; set; }
    }

    private class TestEntity2
    {
        public Guid Id { get; set; }

        public List<TestEntity3> ChildEntities { get; set; }
    }

    private class TestEntity3
    {
        public Guid TestValue1 { get; set; }

        public string TestValue2 { get; set; }

        public bool? TestValue3 { get; set; }

        public DateTime TestValue4 { get; set; }
    }
}
