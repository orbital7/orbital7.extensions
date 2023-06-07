namespace Orbital7.Extensions.ScriptJobs;

public interface IScriptJobsRunnerStartUp
{
    bool UnattendedExecution { get; }

    string WorkingFolderPath { get; }

    ScriptJobBase Create(string[] args);
}
