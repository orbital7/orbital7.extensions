using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbital7.Extensions
{
    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }

        public ProcessResult() { }

        public ProcessResult(int exitCode, string output, string error)
        {
            this.Output = output;
            this.Error = error;
        }
    }
}
