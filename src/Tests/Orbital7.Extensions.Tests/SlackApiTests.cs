using Orbital7.Extensions.Integrations.SlackApi;

namespace Orbital7.Extensions.Tests;


public class SlackApiTests
{
    private string? SlackApiToken { get; set; }

    private string? SlackApiTestChannel { get; set; }

    private IHttpClientFactory HttpClientFactory { get; set; }

    public SlackApiTests()
    {
        var config = ConfigurationHelper.GetConfigurationWithUserSecrets<SlackApiTests>();
        this.SlackApiToken = config["SlackApiToken"];
        this.SlackApiTestChannel = config["SlackApiTestChannel"];
        this.HttpClientFactory = new BasicHttpClientFactory();
    }

    [SkippableFact]
    public async Task TestChatPostMessageApiValid()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.SlackApiToken.HasText() && this.SlackApiTestChannel.HasText());

        // Create the client and service.
        var client = new SlackApiClient(
            this.HttpClientFactory,
            this.SlackApiToken);
        var chatApi = new ChatApi(client);

        // Test.
        var result = await chatApi.PostMessageAsync(new PostMessageRequest()
        {
            Channel = this.SlackApiTestChannel,
            Text = "Sample unit test message post."
        });

        // Validate.
        Assert.NotNull(result);
        Assert.True(result.Ok);
        Assert.Null(result.Error);
        Assert.True(result.Ts.HasText());
        Assert.NotNull(result.Message);
        Assert.True(result.Message.Ts.HasText());
    }

    [SkippableFact]
    public async Task TestChatPostMessageApiInvalid()
    {
        // Skip this test unless we have the necessary configuration data.
        Skip.IfNot(this.SlackApiToken.HasText() && this.SlackApiTestChannel.HasText());

        // Create the client and service.
        var client = new SlackApiClient(
            this.HttpClientFactory,
            this.SlackApiToken);
        var chatApi = new ChatApi(client);

        // Test.
        var result = await chatApi.PostMessageAsync(new PostMessageRequest()
        {
            Channel = this.SlackApiTestChannel + "_INVALID",
            Text = "Sample unit test message post."
        });

        // Validate.
        Assert.NotNull(result);
        Assert.False(result.Ok);
        Assert.True(result.Error.HasText());
    }
}
