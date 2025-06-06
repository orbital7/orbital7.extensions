namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableViewSegment<TEntity>
{
    public string? HeaderText { get; set; }

    public ICollection<TEntity> Items { get; set; }

    public bool HasItems =>
        this.Items != null &&
        this.Items.Count > 0;

    public RATableViewSegment(
        ICollection<TEntity> items)
    {
        this.Items = items;
    }

    public RATableViewSegment(
        string headerText, 
        ICollection<TEntity> items) :
        this(items)
    {
        this.HeaderText = headerText;
    }
}
