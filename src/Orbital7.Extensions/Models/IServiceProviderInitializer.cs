namespace System;

public interface IServiceProviderInitializer
{
    Task InitializeAsync(
        IServiceProvider serviceProvider);
}
