namespace Orbital7.Extensions.ScriptJobs;

public abstract class ScriptJobBase
{
    public virtual string Name => this.GetType().FullName;

    public string WorkingFolderPath { get; internal set; }

    public virtual Task OnLoadAsync()
    {
        return Task.CompletedTask;
    }

    public abstract Task ExecuteAsync();
}
