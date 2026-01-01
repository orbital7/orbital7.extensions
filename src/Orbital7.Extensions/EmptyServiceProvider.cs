namespace Orbital7.Extensions;

public class EmptyServiceProvider :
    IServiceProvider
{
    public object? GetService(
        Type serviceType)
    {
        return null;
    }
}