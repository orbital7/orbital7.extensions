namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableViewSegment<TEntity>
{
    public string? HeaderText { get; init; }

    public IDictionary<int, TEntity> Items { get; init; }

    public bool HasItems =>
        this.Items != null &&
        this.Items.Count > 0;

    public RATableViewSegment(
        ICollection<TEntity> items)
    {
        var i = 0;

        this.Items = items.ToDictionary(
            x => ++i,
            x => x);
    }

    public RATableViewSegment(
        string headerText, 
        ICollection<TEntity> items) :
        this(items)
    {
        this.HeaderText = headerText;
    }
}
