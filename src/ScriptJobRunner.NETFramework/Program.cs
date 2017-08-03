using Orbital7.Extensions.ScriptJobs;
using System;

namespace ScriptJobRunner.NETFramework
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
                Console.WriteLine("ERROR LOADING SCRIPT JOB: " + ex.Message + ex.StackTrace);
            }
            finally
            {
                engine.PressKeyToExit();
            }
        }
    }
}
