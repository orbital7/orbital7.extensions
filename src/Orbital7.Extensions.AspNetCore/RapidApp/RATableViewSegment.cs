namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableViewSegment<TItem>
{
    public string? HeaderText { get; init; }

    public IDictionary<int, TItem> Items { get; init; }

    public bool HasItems =>
        this.Items != null &&
        this.Items.Count > 0;

    public RATableViewSegment(
        ICollection<TItem> items)
    {
        var i = 0;

        this.Items = items.ToDictionary(
            x => ++i,
            x => x);
    }

    public RATableViewSegment(
        string headerText, 
        ICollection<TItem> items) :
        this(items)
    {
        this.HeaderText = headerText;
    }
}
