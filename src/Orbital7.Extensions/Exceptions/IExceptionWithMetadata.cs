namespace Orbital7.Extensions.Exceptions;

public interface IExceptionWithMetadata
{
    IDictionary<string, object?> GetMetadata();
}
