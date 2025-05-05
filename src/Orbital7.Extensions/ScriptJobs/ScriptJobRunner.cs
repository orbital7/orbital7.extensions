namespace Orbital7.Extensions.ScriptJobs;

public static class ScriptJobRunner
{
    public static async Task ExecuteAsync(
        ScriptJobBase scriptJob,
        bool unattendedExecution = false,
        string? workingFolderPath = null)
    {
        // Validate.
        if (scriptJob != null)
        {
            // Set the working folder if specified.
            if (workingFolderPath.HasText())
            {
                scriptJob.WorkingFolderPath = workingFolderPath;
            }

            // Load the script.
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

                // Ensure the working folder exists.
                if (scriptJob.WorkingFolderPath.HasText())
                {
                    FileSystemHelper.EnsureFolderExists(scriptJob.WorkingFolderPath);
                }

                // Execute.
                await scriptJob.ExecuteAsync();
            }
            catch (Exception ex)
            {
                if (!unattendedExecution)
                {
                    ConsoleHelper.WriteExceptionLine(ex, "UNHANDLED EXECUTION ERROR: ");
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            // Display elapsed time.
            Console.WriteLine();
            Console.WriteLine("RUN TIME: " + stopwatch.Elapsed.ToString());
        }
        else
        {
            var errorMessage = "ERROR: No Script Job has been loaded for execution";
            if (!unattendedExecution)
            {
                Console.WriteLine(errorMessage);
            }
            else
            {
                throw new Exception(errorMessage);
            }
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
