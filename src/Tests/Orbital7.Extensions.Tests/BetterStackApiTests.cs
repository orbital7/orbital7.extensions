using Microsoft.Extensions.Configuration;
using Orbital7.Extensions.Integrations.BetterStackApi;
using System.Text.Json.Serialization;

namespace Orbital7.Extensions.Tests;

public class BetterStackApiTests
{
    private string BetterStackUptimeApiToken { get; set; }

    private string BetterStackLogsSourceToken { get; set; }

    private IHttpClientFactory HttpClientFactory { get; set; }

    public BetterStackApiTests()
    {
        var config = ConfigurationHelper.GetConfigurationWithUserSecrets<BetterStackApiTests>();
        this.BetterStackUptimeApiToken = config["BetterStackUptimeApiToken"];
        this.BetterStackLogsSourceToken = config["BetterStackLogsSourceToken"];
        this.HttpClientFactory = new BasicHttpClientFactory();
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
        var client = new BetterStackApiClient(
            this.HttpClientFactory, 
            this.BetterStackUptimeApiToken);
        var uptimeHeartbeatsApi = new UptimeHeartbeatsApi(client);

        // Create a heartbeat.
        const string CREATE_HEARTBEAT_NAME = "Test Heartbeat";
        var createHeartbeatResponse = await uptimeHeartbeatsApi.CreateAsync(
            new HeartbeatRequest()
            {
                Name = CREATE_HEARTBEAT_NAME,
                Period = 60,
                Grace = 12,
                Email = true,
                Paused = false,
            });
        Assert.True(createHeartbeatResponse != null);

        // Validate the heartbeat creation.
        var heartbeat = createHeartbeatResponse.Data;
        Assert.True(heartbeat.Id.HasText());
        Assert.Equal(CREATE_HEARTBEAT_NAME, heartbeat.Attributes.Name);
        Assert.True(heartbeat.Attributes.Status == HeartbeatStatus.Pending ||
            heartbeat.Attributes.Status == HeartbeatStatus.Up);
        Assert.False(heartbeat.Attributes.Paused);

        // Test listing heartbeats.
        var heartbeatsResponse = await uptimeHeartbeatsApi.ListAllExistingAsync();
        Assert.NotNull(heartbeatsResponse.Data.Where(x => x.Id == heartbeat.Id).FirstOrDefault());

        // Send the heartbeat.
        await uptimeHeartbeatsApi.SendAsync(heartbeat.Attributes.Url);

        // Get the heartbeat.
        var getHeartbeatResponse = await uptimeHeartbeatsApi.GetAsync(heartbeat.Id);
        heartbeat = getHeartbeatResponse.Data;
        Assert.True(heartbeat.Attributes.Status == HeartbeatStatus.Pending ||
            heartbeat.Attributes.Status == HeartbeatStatus.Up);
        Assert.False(heartbeat.Attributes.Paused);

        // Update the heartbeat.
        const string UPDATE_HEARTBEAT_NAME = "Test Heartbeat (updated)";
        var updateHeartbeatResponse = await uptimeHeartbeatsApi.UpdateAsync(
            heartbeat.Id,
            new HeartbeatRequest()
            {
                Name = UPDATE_HEARTBEAT_NAME,
                Paused = true,
            });
        Assert.True(updateHeartbeatResponse != null);

        // Validate the heartbeat update.
        heartbeat = updateHeartbeatResponse.Data;
        Assert.Equal(UPDATE_HEARTBEAT_NAME, heartbeat.Attributes.Name);
        Assert.Equal(HeartbeatStatus.Paused, heartbeat.Attributes.Status);
        Assert.True(heartbeat.Attributes.Paused);

        // Delete the heartbeat.
        await uptimeHeartbeatsApi.DeleteAsync(heartbeat.Id);
        heartbeatsResponse = await uptimeHeartbeatsApi.ListAllExistingAsync();
        Assert.Null(heartbeatsResponse.Data.Where(x => x.Id == heartbeat.Id).FirstOrDefault());

        // Test parsing page index.
        var pageIndex = heartbeatsResponse.ParsePageIndex(heartbeatsResponse.Pagination.First);
        Assert.Equal(1, pageIndex);
    }

    [SkippableFact]
    public async Task TestLogsUploadApi()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.BetterStackLogsSourceToken.HasText());

        // Create the client and service.
        var client = new BetterStackApiClient(this.HttpClientFactory);
        var logsUploadApi = new LogsUploadApi(client);

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
        await logsUploadApi.LogEventAsync(
            this.BetterStackLogsSourceToken,
            logEvent);

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
        await logsUploadApi.LogEventsAsync(
            this.BetterStackLogsSourceToken, 
            logEvents);
    }

    private TestEntity1 CreateTestEntity1()
    {
        return new TestEntity1()
        {
            Name = Guid.NewGuid().ToString(),
            TestEnum = TestEnumType.EnumValue2,
            ChildEntity = new TestEntity2()
            {
                Id = Guid.NewGuid(),
                TestEnum = TestEnumType.EnumValue3,
                ChildEntities = new List<TestEntity3>()
                {
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = Guid.NewGuid().ToString(),
                        TestValue3 = true,
                        TestValue4 = DateTime.Now,
                        TestValue5 = TestEnumType.EnumValue2
                    },
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = null,
                        TestValue3 = null,
                        TestValue4 = DateTime.Now,
                        TestValue5 = null,
                    },
                    new TestEntity3()
                    {
                        TestValue1 = Guid.NewGuid(),
                        TestValue2 = Guid.NewGuid().ToString(),
                        TestValue3 = false,
                        TestValue4 = DateTime.UtcNow,
                        TestValue5 = TestEnumType.EnumValue1,
                    },
                            }
            }
        };
    }

    private class TestEntity1
    {
        public string Name { get; set; }

        public TestEntity2 ChildEntity { get; set; }

        public TestEnumType TestEnum { get; set; }
    }

    private class TestEntity2
    {
        public Guid Id { get; set; }

        public TestEnumType TestEnum { get; set; }

        public List<TestEntity3> ChildEntities { get; set; }
    }

    private class TestEntity3
    {
        public Guid TestValue1 { get; set; }

        public string TestValue2 { get; set; }

        public bool? TestValue3 { get; set; }

        public DateTime TestValue4 { get; set; }

        public TestEnumType? TestValue5 { get; set; }
    }

    private enum TestEnumType
    {
        EnumValue1,

        EnumValue2,

        EnumValue3,
    }
}
