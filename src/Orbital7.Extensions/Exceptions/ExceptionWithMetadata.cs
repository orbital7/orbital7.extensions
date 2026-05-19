namespace Orbital7.Extensions.Exceptions;

public class ExceptionWithMetadata :
    Exception, IExceptionWithMetadata
{
    private readonly IDictionary<string, object?> _metadata;

    public ExceptionWithMetadata(
        string message, 
        IDictionary<string, object?> metadata) : 
        base(message)
    {
        _metadata = metadata;
    }

    public ExceptionWithMetadata(
        string message, 
        Exception innerException, 
        IDictionary<string, object?> metadata) : 
        base(message, innerException)
    {
        _metadata = metadata;
    }

    public IDictionary<string, object?> GetMetadata()
    {
        return _metadata;
    }
}
