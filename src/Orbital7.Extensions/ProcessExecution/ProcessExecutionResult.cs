namespace System.Diagnostics;

public class ProcessExecutionResult
{
    public int ExitCode { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }

    public ProcessExecutionResult() { }

    public ProcessExecutionResult(int exitCode, string output, string error)
    {
        this.Output = output;
        this.Error = error;
    }
}
