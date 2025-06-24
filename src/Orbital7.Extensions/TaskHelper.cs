namespace Orbital7.Extensions;

public static class TaskHelper
{
    public static async Task RunWithHeartbeatTimeoutAsync(
        Func<TaskHeartbeatMonitor, CancellationToken, Task> taskFunction,
        TimeSpan timeout,
        string? timeoutExceptionMessage = null,
        CancellationToken? linkedCancellationToken = null)
    {
        using TaskHeartbeatMonitor taskHeartbeatMonitor = new(linkedCancellationToken);

        var task = Task.Run(async () => await taskFunction(
            taskHeartbeatMonitor, 
            taskHeartbeatMonitor.CancellationToken));

        await RunTaskWithHeartbeatTimeoutAsync(
            task, 
            taskHeartbeatMonitor, 
            timeout, 
            timeoutExceptionMessage);
    }

    public static async Task<TResult> RunWithHeartbeatTimeoutAsync<TResult>(
        Func<TaskHeartbeatMonitor, CancellationToken, Task<TResult>> taskFunction,
        TimeSpan timeout,
        string? timeoutExceptionMessage = null,
        CancellationToken? linkedCancellationToken = null)
    {
        using TaskHeartbeatMonitor taskHeartbeatMonitor = new(linkedCancellationToken);

        var task = Task.Run(async () => await taskFunction(
            taskHeartbeatMonitor, 
            taskHeartbeatMonitor.CancellationToken));

        await RunTaskWithHeartbeatTimeoutAsync(
            task, 
            taskHeartbeatMonitor, 
            timeout, 
            timeoutExceptionMessage);

        return task.Result;
    }

    public static async Task RunWithHeartbeatTimeoutAsync<TInput>(
        Func<TInput, TaskHeartbeatMonitor, CancellationToken, Task> taskFunction,
        TInput input,
        TimeSpan timeout,
        string? timeoutExceptionMessage = null,
        CancellationToken? linkedCancellationToken = null)
    {
        using TaskHeartbeatMonitor taskHeartbeatMonitor = new(linkedCancellationToken);

        var task = Task.Run(async () => await taskFunction(
            input, 
            taskHeartbeatMonitor, 
            taskHeartbeatMonitor.CancellationToken));

        await RunTaskWithHeartbeatTimeoutAsync(
            task, 
            taskHeartbeatMonitor, 
            timeout, 
            timeoutExceptionMessage);
    }

    public static async Task<TResult> RunWithHeartbeatTimeoutAsync<TInput, TResult>(
        Func<TInput, TaskHeartbeatMonitor, CancellationToken, Task<TResult>> taskFunction,
        TInput input,
        TimeSpan timeout,
        string? timeoutExceptionMessage = null,
        CancellationToken? linkedCancellationToken = null)
    {
        using TaskHeartbeatMonitor taskHeartbeatMonitor = new(linkedCancellationToken);

        var task = Task.Run(async () => await taskFunction(
            input, 
            taskHeartbeatMonitor, 
            taskHeartbeatMonitor.CancellationToken));

        await RunTaskWithHeartbeatTimeoutAsync(
            task, 
            taskHeartbeatMonitor, 
            timeout, 
            timeoutExceptionMessage);

        return task.Result;
    }

    private static async Task RunTaskWithHeartbeatTimeoutAsync<TTask>(
        TTask task,
        TaskHeartbeatMonitor taskHeartbeatMonitor,
        TimeSpan timeout,
        string? timeoutExceptionMessage)
        where TTask : Task
    {
        // We're looping until either the task is completed or if we
        // don't receive a heartbeat signal within the timeout period,
        // in which case we throw a TimeoutException.
        while (!task.IsCompleted)
        {
            // Reset the task heartbeat monitor.
            taskHeartbeatMonitor.Reset();

            // Create the timeout task.
            using var timeoutlinkedCancellationTokenSource = new CancellationTokenSource();
            var timeoutTask = Task.Delay(
                timeout,
                timeoutlinkedCancellationTokenSource.Token);

            // Wait for either the primary task, the timeout task, or the
            // heartbeat monitor task to complete.
            var completedTask = await Task.WhenAny(
                task,
                timeoutTask,
                taskHeartbeatMonitor.HeartbeatSource.Task);

            // If the completed task is the timeout task, then we 
            // throw the timeout exception.
            if (completedTask == timeoutTask)
            {
                // Cancel the heartbeat and timeout tasks.
                taskHeartbeatMonitor.CancellationTokenSource.Cancel();
                timeoutlinkedCancellationTokenSource.Cancel();

                // Throw the exception.
                throw new TimeoutException(timeoutExceptionMessage);
            }
            // Else if the completed task is the primary task, then
            // we're finished and we break out of the loop.
            else if (completedTask == task)
            {
                // Cancel the timeout task.
                timeoutlinkedCancellationTokenSource.Cancel();

                // Propagate any exceptions that occurred in the task
                // and then break out of the loop.
                await task;
                break;
            }

            // Else the completed task is the heartbeat monitor task,
            // which means we have received a heartbeat signal, so 
            // we're good to create a new heart monitor and timeout
            // task (the primary task just keeps running).
        }
    }
}
