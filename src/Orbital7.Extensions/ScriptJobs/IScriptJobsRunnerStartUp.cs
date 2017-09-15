using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.ScriptJobs
{
    public interface IScriptJobsRunnerStartUp
    {
        bool UnattendedExecution { get; }

        string WorkingFolderPath { get; }

        ScriptJob Create(string[] args);
    }
}
