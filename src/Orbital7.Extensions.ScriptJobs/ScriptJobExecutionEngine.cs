using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.ScriptJobs
{
    public class ScriptJobExecutionEngine
    {
        public const string ARG_UNATTENDED = "-UNATTENDED";

        public ScriptJob ScriptJob { get; private set; }
        public ScriptJobExecutionSettings Settings { get; private set; }

        public ScriptJobExecutionEngine()
        {
            this.Settings = new ScriptJobExecutionSettings()
            {
                UnattendedExecution = false,
            };
        }

        public void Load(string[] args)
        {
            // Validate and load.
            if (args.Length >= 1 && File.Exists(args[0]))
                Load(XMLSerializationHelper.LoadFromXML<ScriptJobExecutionSettings>(File.ReadAllText(args[0])));
            else if (args.Length < 2)
                throw new Exception("Insufficient arguments provided: [AssemblyName] [TypeName]");
            else
                Load(args[0], args[1], args.Length >= 3 ? args[2] : String.Empty);

            // Allow for unattended execution override at the command line level.
            if (ContainsUnattendedArg(args))
                this.Settings.UnattendedExecution = true;
        }

        public void Load(ScriptJobExecutionSettings executionSettings)
        {
            this.Settings = executionSettings;
            Load(this.Settings.AssemblyName, this.Settings.TypeName, this.Settings.WorkingFolderPath);
        }

        public void Load(string assemblyName, string typeName, string workingFolderPath = null)
        {
            Load(ReflectionHelper.CreateInstance<ScriptJob>(assemblyName, typeName), workingFolderPath);
        }

        public void Load(ScriptJob scriptJob, string workingFolderPath = null)
        {
            // Validate/Record.
            this.ScriptJob = scriptJob ?? throw new Exception("Provided ScriptJob is NULL");

            // Set working folder.
            if (!String.IsNullOrEmpty(workingFolderPath))
                this.ScriptJob.WorkingFolderPath = workingFolderPath;
            else
                this.ScriptJob.WorkingFolderPath = ReflectionHelper.GetExecutingAssemblyFolderPath();
        }

        public void Execute()
        {
            Task.Run(async () => { await ExecuteAsync(); }).Wait();
        }

        public async Task ExecuteAsync()
        {
            // Validate.
            if (this.ScriptJob != null)
            {
                Console.WriteLine("LOADED SCRIPT: " + this.ScriptJob.Name);

                // Confirm if requested.
                if (!this.Settings.UnattendedExecution && ConsoleHelper.PressEnterOrEscKey(enterVerb: "begin execution") == ConsoleEnterOrEscKeyResult.Escape)
                {
                    this.Settings.UnattendedExecution = true;
                    return;
                }

                // Prepare the stopwatch.
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Execute.
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("RUNNING SCRIPT...");
                    Console.WriteLine();

                    await this.ScriptJob.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteExceptionLine(ex, "UNHANDLED EXECUTION ERROR: ");
                }

                // Display elapsed time.
                stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("RUN TIME: " + stopwatch.Elapsed.ToString());
            }
            else
            {
                Console.WriteLine("ERROR: No Script Job has been loaded for execution");
            }
        }

        public void PressKeyToExit()
        {
            if (!this.Settings.UnattendedExecution)
            {
                ConsoleHelper.PressKeyToContinue("exit");
            }
        }

        public static bool ContainsUnattendedArg(string[] args)
        {
            foreach (var arg in args)
                if (arg.ToUpper() == ARG_UNATTENDED)
                    return true;

            return false;
        }
    }
}
