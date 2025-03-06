namespace Orbital7.Extensions.Notifications;

public class ConsoleExternalNotificationService :
    IExternalNotificationService
{
    public async Task<bool> SendTraceAsync(
        string message)
    {
        return await SendAsync("#trace", message);
    }

    public async Task<bool> SendDebugAsync(
        string message)
    {
        return await SendAsync("#debug", message);
    }

    public async Task<bool> SendInformationAsync(
        string message)
    {
        return await SendAsync("#information", message);
    }

    public async Task<bool> SendWarningAsync(
        string message)
    {
        return await SendAsync("#warning", message);
    }

    public async Task<bool> SendErrorAsync(
        string message)
    {
        return await SendAsync("#error", message);
    }

    public async Task<bool> SendCriticalAsync(
        string message)
    {
        return await SendAsync("#critical", message);
    }

    private async Task<bool> SendAsync(
        string channel, 
        string message)
    {
        Console.WriteLine($"{channel}: {message}");
        return await Task.FromResult(true);
    }
}
