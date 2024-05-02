using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Orbital7.Extensions.AspNetCore;

public sealed class SetBrowserTimeConverter : 
    ComponentBase
{
    [Inject] 
    public TimeConverter TimeConverter { get; set; } = default!;

    [Inject] 
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(
        bool firstRender)
    {
        if (firstRender && 
            !this.TimeConverter.IsCustomLocalTimeZoneSet)
        {
            try
            {
                await using var module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "import",
                    "./_content/Orbital7.Extensions.AspNetCore/browserTimeZone.js");
                var timeZone = await module.InvokeAsync<string>("getBrowserTimeZone");
                this.TimeConverter.SetCustomLocalTimeZone(timeZone);
            }
            catch (JSDisconnectedException)
            {
            }
        }
    }
}