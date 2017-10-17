using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.ScriptJobs
{
    public class ScriptJobExecutionEngine
    {
        public const string ARG_UNATTENDED = "-UNATTENDED";
        public const string ARG_FILE = "-FILE";
        public const string ARG_ASSEMBLY = "-ASSEMBLY";

        public ScriptJob ScriptJob { get; private set; }

        public ScriptJobExecutionSettings Settings { get; private set; }

        public ScriptJobExecutionEngine()
        {
            this.Settings = new ScriptJobExecutionSettings()
            {
                UnattendedExecution = false,
            };
        }
        
        public ScriptJobExecutionEngine Load(string[] args)
        {
            // Validate and load.
            if (args.Length >= 1 && args[0].ToUpper() == ARG_FILE)
            {
                if (args.Length >= 2 && File.Exists(args[1]))
                    Load(SerializationHelper.LoadFromXml<ScriptJobExecutionSettings>(File.ReadAllText(args[1])));
                else
                    throw new Exception("FILE USAGE: -FILE [FilePath]");
            }
            else if (args.Length >= 1 && args[0].ToUpper() == ARG_ASSEMBLY)
            {
                if (args.Length >= 3)
                    Load(args[1], args[2], args.Length >= 4 ? args[3] : String.Empty);
                else
                    throw new Exception("ASSEMBLY USAGE: -ASSEMBLY [AssemblyName] [TypeName] [Opt: WorkingFolderPath]");
            }
            else
            {
                var startUp = GetStartUp();
                if (startUp != null)
                    Load(startUp, args);
                else
                    throw new Exception("No Script Job start sequence was found");
            }

            // Allow for unattended execution override at the command line level.
            if (ContainsUnattendedArg(args))
                this.Settings.UnattendedExecution = true;

            return this;
        }

        private IScriptJobsRunnerStartUp GetStartUp()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes<IScriptJobsRunnerStartUp>();
            if (types.Count > 0)
                return types[0].CreateInstance<IScriptJobsRunnerStartUp>();
            else
                return null;
        }

        public ScriptJobExecutionEngine Load(IScriptJobsRunnerStartUp startUp, string[] args)
        {
            this.Settings.UnattendedExecution = startUp.UnattendedExecution;
            Load(startUp.Create(args), startUp.WorkingFolderPath);
            return this;
        }

        public ScriptJobExecutionEngine Load(ScriptJobExecutionSettings executionSettings)
        {
            this.Settings = executionSettings;
            return Load(this.Settings.AssemblyName, this.Settings.TypeName, this.Settings.WorkingFolderPath);
        }

        public ScriptJobExecutionEngine Load(string assemblyName, string typeName, string workingFolderPath = null)
        {
            return Load(ReflectionHelper.CreateInstance<ScriptJob>(assemblyName, typeName), workingFolderPath);
        }

        public ScriptJobExecutionEngine Load(ScriptJob scriptJob, string workingFolderPath = null)
        {
            // Validate/Record.
            this.ScriptJob = scriptJob ?? throw new Exception("Provided ScriptJob is NULL");

            // Set working folder.
            if (!String.IsNullOrEmpty(workingFolderPath))
                this.ScriptJob.WorkingFolderPath = workingFolderPath;
            else
                this.ScriptJob.WorkingFolderPath = ReflectionHelper.GetExecutingAssemblyFolderPath();

            return this;
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

            PressKeyToExit();
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

        public static void Execute(string[] args)
        {
            new ScriptJobExecutionEngine().Load(args).Execute();
        }

        public static void Execute(IScriptJobsRunnerStartUp startUp, string[] args)
        {
            new ScriptJobExecutionEngine().Load(startUp, args).Execute();
        }

        public static void Execute(ScriptJobExecutionSettings executionSettings)
        {
            new ScriptJobExecutionEngine().Load(executionSettings).Execute();
        }

        public static void Execute(string assemblyName, string typeName, string workingFolderPath = null)
        {
            new ScriptJobExecutionEngine().Load(assemblyName, typeName, workingFolderPath).Execute();
        }

        public static void Execute(ScriptJob scriptJob, string workingFolderPath = null)
        {
            new ScriptJobExecutionEngine().Load(scriptJob, workingFolderPath).Execute();
        }

        public static async Task ExecuteAsync(string[] args)
        {
            await new ScriptJobExecutionEngine().Load(args).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IScriptJobsRunnerStartUp startUp, string[] args)
        {
            await new ScriptJobExecutionEngine().Load(args).ExecuteAsync();
        }

        public static async Task ExecuteAsync(ScriptJobExecutionSettings executionSettings)
        {
            await new ScriptJobExecutionEngine().Load(executionSettings).ExecuteAsync();
        }

        public static async Task ExecuteAsync(string assemblyName, string typeName, string workingFolderPath = null)
        {
            await new ScriptJobExecutionEngine().Load(assemblyName, typeName, workingFolderPath).ExecuteAsync();
        }

        public static async Task ExecuteAsync(ScriptJob scriptJob, string workingFolderPath = null)
        {
            await new ScriptJobExecutionEngine().Load(scriptJob, workingFolderPath).ExecuteAsync();
        }
    }
}
