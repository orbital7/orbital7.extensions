using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Orbital7.Extensions.ScriptJobs
{
    public abstract class ScriptJob
    {
        public virtual string Name
        {
            get { return this.GetType().FullName; }
        }

        public string WorkingFolderPath { get; internal set; }

        public abstract Task ExecuteAsync();
    }
}
