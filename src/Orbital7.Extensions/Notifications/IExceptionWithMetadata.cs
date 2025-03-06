namespace Orbital7.Extensions.Notifications;

public interface IExceptionWithMetadata
{
    IDictionary<string, object> GetMetadata();
}
