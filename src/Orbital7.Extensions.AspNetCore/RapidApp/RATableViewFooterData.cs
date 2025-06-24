namespace Orbital7.Extensions.AspNetCore.RapidApp;

public record RATableViewFooterData<TItem>
{
    public required RATableTemplate<TItem>.Column<TItem> Column { get; init; }

    public required List<RATableViewSegment<TItem>> Segments { get; init; }

    public required List<TItem> AllItems { get; init; }
}