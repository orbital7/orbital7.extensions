namespace Orbital7.Extensions;

public static class TaskExtensions
{
    public static async Task RunWithTimeoutAsync(
        this Task task,
        TimeSpan timeout,
        CancellationTokenSource? taskCancellationTokenSource = null,
        string? timeoutExceptionMessage = null)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(
            task,
            Task.Delay(
                timeout, 
                timeoutCancellationTokenSource.Token));

        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task;
        }
        else
        {
            if (taskCancellationTokenSource != null)
            {
                taskCancellationTokenSource.Cancel();
            }

            throw new TimeoutException(timeoutExceptionMessage);
        }
    }

    // Source: https://github.com/brminnick/AsyncAwaitBestPractices
    /// <summary>
    /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
    /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
    public static async void SafeFireAndForget(
        this Task task, 
        bool continueOnCapturedContext = true, 
        Action<Exception>? onException = null)
        #pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex) when (onException != null)
        {
            onException(ex);
        }
    }

    // Source: https://github.com/brminnick/AsyncAwaitBestPractices
    /// <summary>
    /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
    /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
    public static async void SafeFireAndForget(
        this Task task, 
        bool continueOnCapturedContext = true, 
        Func<Exception, Task>? onException = null)
        #pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex) when (onException != null)
        {
            await onException(ex);
        }
    }
}
