using System.Diagnostics;

namespace Orbital7.Extensions.ScriptJobs;

public static class ScriptJobExecutionEngine
{
    public static async Task ExecuteAsync(
        ScriptJobBase scriptJob,
        bool unattendedExecution = false,
        string? workingFolderPath = null)
    {
        // Validate.
        if (scriptJob != null)
        {
            // Load the script.
            scriptJob.WorkingFolderPath = workingFolderPath;
            await scriptJob.OnLoadAsync();
            Console.WriteLine("LOADED SCRIPT: " + scriptJob.Name);

            // Confirm execution if not unattended.
            if (!unattendedExecution &&
                ConsoleHelper.PressEnterOrEscKey(enterVerb: "begin execution") == ConsoleEnterOrEscKeyResult.Escape)
            {
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

                await scriptJob.ExecuteAsync();
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

        // Present exit confirmation.
        if (!unattendedExecution)
        {
            ConsoleHelper.PressKeyToContinue("exit");
        }
    }

    public static async Task ExecuteAsync<TScriptJob>(
        bool unattendedExecution = false,
        string? workingFolderPath = null)
        where TScriptJob : ScriptJobBase, new()
    {
        var scriptJob = new TScriptJob();
        await ExecuteAsync(
            scriptJob,
            unattendedExecution,
            workingFolderPath);
    }
}
