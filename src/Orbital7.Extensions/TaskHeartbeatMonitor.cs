namespace Orbital7.Extensions;

public class TaskHeartbeatMonitor : 
    IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;
    private TaskCompletionSource _heartbeatSource;

    internal TaskCompletionSource HeartbeatSource => _heartbeatSource;

    internal CancellationTokenSource CancellationTokenSource => _cancellationTokenSource;

    internal CancellationToken CancellationToken => _cancellationTokenSource.Token;

    public TaskHeartbeatMonitor(
        CancellationToken? linkedCancellationToken = null)
    {
        if (linkedCancellationToken == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        else
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                linkedCancellationToken.Value);
        }

        _heartbeatSource = new();
    }

    internal void Reset()
    {
        _heartbeatSource = new();
    }

    public void SignalHeartbeat()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _heartbeatSource?.TrySetResult();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
