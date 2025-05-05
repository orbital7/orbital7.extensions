namespace Orbital7.Extensions.ScriptJobs;

public abstract class SynchronousScriptJobBase : 
    ScriptJobBase
{
    protected abstract void Execute();

    public sealed override Task ExecuteAsync()
    {
        return Task.Run(() => Execute());
    }
}