namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableViewFooterData<TItem>
{
    public RATableTemplate<TItem>.Column<TItem>? Column { get; set; }

    public List<RATableViewSegment<TItem>>? Segments { get; set; }
}
