using Orbital7.Extensions.Apis.DiscordApi;

namespace Orbital7.Extensions.Tests;

public class DiscordApiTests
{
    private string? DiscordApiBotToken { get; set; }

    private ulong? DiscordApiTestChannelId { get; set; }

    private IHttpClientFactory HttpClientFactory { get; set; }

    public DiscordApiTests()
    {
        var config = ConfigurationHelper.GetConfigurationWithUserSecrets<DiscordApiTests>();
        this.DiscordApiBotToken = config["DiscordApiBotToken"];
        this.DiscordApiTestChannelId = StringHelper.ParseNullableULong(config["DiscordApiTestChannelId"]);
        this.HttpClientFactory = new BasicHttpClientFactory();
    }

    [SkippableFact]
    public async Task TestChannelsCreateMessageApiValid()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.DiscordApiBotToken.HasText() && this.DiscordApiTestChannelId.HasValue);

        // Create the client and service.
        var client = new DiscordApiClient(
            this.HttpClientFactory,
            this.DiscordApiBotToken);
        var channelsApi = new ChannelsApi(client);

        // Test.
        var result = await channelsApi.CreateMessageAsync(
             this.DiscordApiTestChannelId.Value,
             new CreateMessageRequest()
            {
                Content = "Sample unit test message post."
            });

        // Validate.
        Assert.NotNull(result);
        Assert.True(result.Id.HasText());
        Assert.True(result.ChannelId.HasText());
        Assert.True(result.Content.HasText());
        Assert.True(result.Timestamp.HasText());
    }

    [SkippableFact]
    public async Task TestChannelsCreateMessageApiInvalid()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.DiscordApiBotToken.HasText() && this.DiscordApiTestChannelId.HasValue);

        // Create the client and service.
        var client = new DiscordApiClient(
            this.HttpClientFactory,
            this.DiscordApiBotToken);
        var channelsApi = new ChannelsApi(client);

        // Test and validate.
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await channelsApi.CreateMessageAsync(
                0,  // Invalid channel ID.
                new CreateMessageRequest()
                {
                    Content = "Sample unit test message that should never be posted..."
                });
        });
        Assert.True(exception.Message.HasText());
    }
}
