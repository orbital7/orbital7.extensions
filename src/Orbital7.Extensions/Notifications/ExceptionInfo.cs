namespace Orbital7.Extensions.Notifications;

public class ExceptionInfo
{
    public string? Type { get; set; }

    public string? Source { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public ExceptionInfo? InnerException { get; set; }

    public IDictionary<string, object>? Metadata { get; set; }

    public ExceptionInfo()
    {
        
    }

    public ExceptionInfo(
        Exception ex)
    {
        this.Type = ex.GetType().FullName ?? ex.GetType().Name;
        this.Source = ex.Source;
        this.Message = ex.Message;
        this.StackTrace = ex.StackTrace;

        if (ex is IExceptionWithMetadata)
        {
            this.Metadata = ((IExceptionWithMetadata)ex).GetMetadata();
        }
        // TODO: Should we use this? What happens if "data" is a non-seriazable object?
        //else if (ex.Data != null && ex.Data.Count > 0)
        //{
        //    this.Metadata = new Dictionary<string, object>();

        //    int index = 0;
        //    foreach (var data in ex.Data)
        //    {
        //        index++;
        //        this.Metadata.Add($"Data{index}", data);
        //    }
        //}

        if (ex.InnerException != null)
        {
            this.InnerException = new ExceptionInfo(ex.InnerException);
        }
    }
}
