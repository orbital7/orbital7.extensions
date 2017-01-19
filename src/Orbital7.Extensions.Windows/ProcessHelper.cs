using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Orbital7.Extensions.Windows
{
    public static class ProcessHelper
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        public static bool IsProcess32Bit(Process process)
        {
            if (!Environment.Is64BitOperatingSystem)
                return true;

            bool isWow64Process;
            if (!IsWow64Process(process.Handle, out isWow64Process))
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

            return isWow64Process;
        }

        public static bool IsCurrentProcess32Bit()
        {
            if (IntPtr.Size == 4)
                return true;
            else
                return false;
        }

        public static Process IsProcessAlreadyRunning()
        {
            Process alreadyRunningProcess = null;
            Process thisProcess = Process.GetCurrentProcess();

            foreach (Process process in Process.GetProcessesByName(thisProcess.ProcessName))
            {
                if (process.Id != thisProcess.Id)
                {
                    alreadyRunningProcess = process;
                    break;
                }
            }

            return alreadyRunningProcess;
        }

        public static void OpenFileViaShell(string filePath)
        {
            OpenFileViaShell(filePath, false);
        }

        public static void OpenFileViaShell(string filePath, bool waitForExit)
        {
            OpenFileViaShell(filePath, String.Empty, waitForExit);
        }

        public static void OpenFileViaShell(string filePath, string commandLineArgs, bool waitForExit)
        {
            try
            {
                var process = new Process();
                process.EnableRaisingEvents = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = filePath;
                process.StartInfo.Arguments = commandLineArgs;
                process.Start();
                if (waitForExit) process.WaitForExit();
            }
            catch { }
        }
        
        public static ProcessResult GetProcessOutput(string fileName, string commandLineArgs)
        {
            return GetProcessOutput(fileName, commandLineArgs, 0);
        }

        public static ProcessResult GetProcessOutput(string fileName, string commandLineArgs, int secondsToTimeout)
        {
            var processOutput = new ProcessResult();

            // Create the process.
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = fileName;
            processStartInfo.Arguments = commandLineArgs;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;

            // Start the process.
            var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = false;
            process.Start();

            // Handle timeout.
            if (secondsToTimeout == 0)
            {
                process.WaitForExit();
            }
            else
            {
                process.WaitForExit(1000 * secondsToTimeout);
                if (!process.HasExited)
                    process.Kill();
            }

            // Record output.
            processOutput.ExitCode = process.ExitCode;
            processOutput.Output = process.StandardOutput.ReadToEnd();
            processOutput.Error = process.StandardError.ReadToEnd();

            return processOutput;
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs)
        {
            return ExecuteProcess(fileName, commandLineArgs, String.Empty);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, bool windowVisible)
        {
            return ExecuteProcess(fileName, commandLineArgs, String.Empty, windowVisible);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, string workingFolder)
        {
            return ExecuteProcess(fileName, commandLineArgs, workingFolder, null, false);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, string workingFolder, bool windowVisible)
        {
            return ExecuteProcess(fileName, commandLineArgs, workingFolder, 0, windowVisible);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, string workingFolder, int secondsToTimeout)
        {
            return CreateProcess(fileName, commandLineArgs, workingFolder, null, secondsToTimeout, false);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, string workingFolder, int secondsToTimeout, bool windowVisible)
        {
            return CreateProcess(fileName, commandLineArgs, workingFolder, null, secondsToTimeout, windowVisible);
        }

        public static int ExecuteProcess(string fileName, string commandLineArgs, string workingFolder, ProcessThreadCompleteHandler completeHandler, bool windowVisible)
        {
            return CreateProcess(fileName, commandLineArgs, workingFolder, completeHandler, 0, windowVisible);
        }

        public static void ExecuteProcessInSeparateThread(string fileName, string commandLineArgs, ProcessThreadCompleteHandler completeHandler)
        {
            ExecuteProcessInSeparateThread(new ProcessStartInfo(fileName, commandLineArgs), completeHandler);
        }

        public static void ExecuteProcessInSeparateThread(ProcessStartInfo processStartInfo, ProcessThreadCompleteHandler completeHandler)
        {
            ExecuteProcessInSeparateThread(processStartInfo, 0, null, completeHandler);
        }

        public static void ExecuteProcessInSeparateThread(ProcessStartInfo processStartInfo, 
            ProcessWriteLineRedirectDelegate processWriteLineRedirectDelegate, ProcessThreadCompleteHandler completeHandler)
        {
            ExecuteProcessInSeparateThread(processStartInfo, 0, processWriteLineRedirectDelegate, completeHandler);
        }

        public static ProcessThread ExecuteProcessInSeparateThread(ProcessStartInfo processStartInfo, int maxStackSizeInMB, 
            ProcessWriteLineRedirectDelegate processWriteLineRedirectDelegate, ProcessThreadCompleteHandler completeHandler)
        {
            var processThread = new ProcessThread(processStartInfo, completeHandler, processWriteLineRedirectDelegate);

            Thread thread = null;
            if (maxStackSizeInMB > 0)
                thread = new Thread(new ThreadStart(processThread.Start), maxStackSizeInMB * 1024 * 1024);
            else
                thread = new Thread(new ThreadStart(processThread.Start));

            thread.Start();

            return processThread;
        }

        private static int CreateProcess(string fileName, string commandLineArgs, string workingFolder, ProcessThreadCompleteHandler completeHandler, int secondsToTimeout, bool windowVisible)
        {
            // Create the process.
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = fileName;
            processStartInfo.Arguments = commandLineArgs;
            processStartInfo.WorkingDirectory = workingFolder;

            // Set window visibility.
            if (windowVisible)
            {
                processStartInfo.CreateNoWindow = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            }
            else
            {
                processStartInfo.CreateNoWindow = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }

            // If running the complete handler, start the process as a new thread.
            if (completeHandler != null)
            {
                ExecuteProcessInSeparateThread(processStartInfo, completeHandler);
                return 0;
            }
            // Else create and start the process.
            else
            {
                var process = new Process();
                process.StartInfo = processStartInfo;
                process.EnableRaisingEvents = false;
                process.Start();

                if (secondsToTimeout == 0)
                {
                    process.WaitForExit();
                }
                else
                {
                    process.WaitForExit(1000 * secondsToTimeout);
                    if (!process.HasExited) process.Kill();
                }

                return process.ExitCode;
            }
        }
    }
}
