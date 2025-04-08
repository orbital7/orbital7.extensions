namespace Orbital7.Extensions.ScriptJobs;

public abstract class ScriptJobBase
{
    public virtual string Name => this.GetType().FullName ?? this.GetType().Name;

    public string? WorkingFolderPath { get; internal set; }

    public virtual Task OnLoadAsync()
    {
        return Task.CompletedTask;
    }

    public abstract Task ExecuteAsync();
}
