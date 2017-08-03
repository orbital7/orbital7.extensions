using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.ScriptJobs
{
    public class ScriptJobExecutionSettings
    {
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public bool UnattendedExecution { get; set; }
        public string WorkingFolderPath { get; set; }
    }
}
