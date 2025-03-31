namespace Orbital7.Extensions;

public interface IServiceProviderInitializer
{
    Task InitializeAsync(
        IServiceProvider serviceProvider);
}
