using Orbital7.Extensions;
using Orbital7.Extensions.ScriptJobs;
using System;

namespace ScriptJobRunner.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ScriptJobExecutionEngine();

            try
            {
                engine.Load(args);
                engine.Execute();
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteExceptionLine(ex, "ERROR LOADING SCRIPT JOB: ");
            }
            finally
            {
                engine.PressKeyToExit();
            }
        }
    }
}
