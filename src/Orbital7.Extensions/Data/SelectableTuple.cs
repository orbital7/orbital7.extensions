namespace Orbital7.Extensions.Data;

// TODO: Determine if we end up needing this for RapidApp.
public class SelectableTuple<T1, T2> : Tuple<T1, T2>
{
    public bool CanSelect { get; set; } = true;

    public bool IsSelected { get; set; } = false;

    public SelectableTuple(T1 item1, T2 item2)
        : base(item1, item2)
    {

    }

    public override string ToString()
    {
        return string.Format("{0} {1}", this.Item2.ToString(), this.IsSelected ? "[Selected]" : null).Trim();
    }
}
