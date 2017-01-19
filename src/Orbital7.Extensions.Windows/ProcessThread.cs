using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Orbital7.Extensions.Windows
{
    public delegate void ProcessThreadCompleteHandler(int exitCode);
    public delegate void ProcessWriteLineRedirectDelegate(string line);

    public class ProcessThread
    {
        private object m_lockObject = new object();

        private ProcessThreadCompleteHandler CompleteHandler { get; set; }
        private ProcessWriteLineRedirectDelegate WriteLineRedirect { get; set; }
        private ProcessStartInfo StartInfo { get; set; }
        private Process Process { get; set; }
        private int TimeoutInSeconds { get; set; }

        public ProcessThread(ProcessStartInfo startInfo, ProcessThreadCompleteHandler completeHandler)
        {
            this.StartInfo = startInfo;
            this.CompleteHandler = completeHandler;
            this.TimeoutInSeconds = 0;
        }

        public ProcessThread(ProcessStartInfo startInfo, ProcessThreadCompleteHandler completeHandler, int secondsToTimeout)
            : this(startInfo, completeHandler)
        {
            this.TimeoutInSeconds = secondsToTimeout;
        }

        public ProcessThread(ProcessStartInfo startInfo, ProcessThreadCompleteHandler completeHandler, int secondsToTimeout, ProcessWriteLineRedirectDelegate processWriteLineRedirectDelegate)
            : this(startInfo, completeHandler, secondsToTimeout)
        {
            this.WriteLineRedirect = processWriteLineRedirectDelegate;
        }

        public ProcessThread(ProcessStartInfo startInfo, ProcessThreadCompleteHandler completeHandler, ProcessWriteLineRedirectDelegate processWriteLineRedirectDelegate)
            : this(startInfo, completeHandler, 0, processWriteLineRedirectDelegate) { }

        public void Start()
        {
            // Create the process.
            this.Process = new Process();
            this.Process.StartInfo = this.StartInfo;

            // Setup to redirect.
            if (this.WriteLineRedirect != null)
            {
                this.StartInfo.CreateNoWindow = true;
                this.Process.StartInfo.UseShellExecute = false;
                this.Process.StartInfo.RedirectStandardOutput = true;
                this.Process.StartInfo.RedirectStandardError = true;
                this.Process.EnableRaisingEvents = false;
                this.Process.OutputDataReceived += new DataReceivedEventHandler(Process_DataReceived);
                this.Process.ErrorDataReceived += new DataReceivedEventHandler(Process_DataReceived);
            }

            // Start and wait for exit.
            this.Process.Start();

            // Begin reading.
            if (this.WriteLineRedirect != null)
            {
                this.Process.BeginErrorReadLine();
                this.Process.BeginOutputReadLine();
            }

            // Wait for exit.
            if (this.TimeoutInSeconds == 0)
            {
                this.Process.WaitForExit();
            }
            else
            {
                this.Process.WaitForExit(1000 * this.TimeoutInSeconds);
                if (!this.Process.HasExited) this.Process.Kill();
            }

            // Notify complete.
            NotifyCompletion();
        }

        void Process_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.WriteLineRedirect(e.Data);
        }

        public void Kill()
        {
            // Kill.
            this.Process.Kill();

            // Notify complete.
            NotifyCompletion();
        }

        private void NotifyCompletion()
        {
            if (this.CompleteHandler != null)
                this.CompleteHandler(this.Process.ExitCode);
        }
    }
}
