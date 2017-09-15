using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.ScriptJobs
{
    public abstract class SynchronousScriptJob : ScriptJob
    {
        protected abstract void Execute();

        public sealed override Task ExecuteAsync()
        {
            return Task.Run(() => Execute());
        }
    }
}
