namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableViewSegment<TEntity>
{
    public string Name { get; set; }

    public ICollection<TEntity> Items { get; set; }

    public bool HasItems =>
        this.Items != null &&
        this.Items.Count > 0;

    public RATableViewSegment(
        string name, 
        ICollection<TEntity> items)
    {
        this.Name = name;
        this.Items = items;
    }
}
