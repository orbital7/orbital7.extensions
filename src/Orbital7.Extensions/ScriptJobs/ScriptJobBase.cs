namespace Orbital7.Extensions.ScriptJobs;

public abstract class ScriptJobBase
{
    public virtual string Name
    {
        get { return this.GetType().FullName; }
    }

    public string WorkingFolderPath { get; internal set; }

    public abstract Task ExecuteAsync();
}
