namespace Orbital7.Extensions.ProcessExecution;

public class ProcessExecutionResult
{
    public int? ExitCode { get; set; }

    public string? Output { get; set; }

    public string? Error { get; set; }
}
