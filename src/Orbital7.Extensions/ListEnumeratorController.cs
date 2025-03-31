namespace Orbital7.Extensions;

public class ListEnumeratorController<T>
{
    private IEnumerator ItemsEnumerator { get; set; }
    private object LockObject { get; set; }
    public bool IsDone { get; set; }

    public ListEnumeratorController(IEnumerable list)
    {
        // Record.
        this.IsDone = false;
        this.ItemsEnumerator = list.GetEnumerator();

        // Create.
        this.LockObject = new object();
    }

    public T GetNextItem()
    {
        lock (this.LockObject)
        {
            if (this.ItemsEnumerator.MoveNext())
            {
                return (T)this.ItemsEnumerator.Current;
            }
            else
            {
                this.IsDone = true;
                return default(T);
            }
        }
    }
}
