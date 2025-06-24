using Microsoft.JSInterop;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

public static class RAClipboardHelper
{
    public static async Task SetTextAsync(
        IJSRuntime jsRuntime,
        string? text)
    {
        if (text.HasText())
        {
            await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/Orbital7.Extensions.AspNetCore/rapidapp/clipboardHelper.js");

            await module.InvokeAsync<string>("copyTextToClipboard", text);
        }
    }

    public static async Task<string> GetTextAsync(
        IJSRuntime jsRuntime)
    {
        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/Orbital7.Extensions.AspNetCore/rapidapp/clipboardHelper.js");

        return await module.InvokeAsync<string>("pasteTextFromClipboard");
    }
}
