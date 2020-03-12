using System;
using SD = System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    /// <summary>
    /// Process wrapper class
    /// </summary>
    internal class ProcessWrapper
    {
        private SD.Process _process;
        private TimeSpan _totalProcessorTime;
        private SD.Stopwatch _watch;

        /// <summary>
        /// Constructor for the process wrapper
        /// </summary>
        /// <param name="Process"></param>
        public ProcessWrapper(SD.Process Process)
        {
            _process = Process;
            parseProcess();
            onCreated();
        }

        /// <summary>
        /// Get or set the Pid
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// Get or set the process name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the thread count
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// Get or set the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Get or set the user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get or set the memory usage in mega bytes
        /// </summary>
        public long MemoryUsageMB { get; set; }

        /// <summary>
        /// Get or set the CPU usage
        /// </summary>
        public double CpuUsage { get; set; } = 0;

        /// <summary>
        /// Returns a nicely formated string
        /// </summary>
        /// <returns>The formated string</returns>
        public override string ToString()
        {
            return $"{CpuUsage} %, {Name} {MemoryUsageMB} Mb (owner: {UserName})";
        }

        private void parseProcess()
        {
            Name = _process.ProcessName;
            Pid = _process.Id;
            ThreadCount = _process.Threads.Count;
            MemoryUsageMB = _process.WorkingSet64 / 1024 / 1024;

            try
            {
                FileName = _process.MainModule.FileName;
                UserName = retrieveUserName();
            }
            catch
            {
                FileName = "";
                UserName = "SYSTEM";
            }
        }

        private string retrieveUserName()
        {
            IntPtr buffer;
            int strLen;
            string username = string.Empty;
            if (WtsApi32.WTSQuerySessionInformation(IntPtr.Zero, _process.SessionId, WtsApi32.WtsInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer);
                WtsApi32.WTSFreeMemory(buffer);
            }
            return username;
        }

        private void onCreated()
        {
            // Does not work for system process with managed code
            if (UserName != "SYSTEM")
            {
                _watch = new SD.Stopwatch();
                _watch.Start();
                _totalProcessorTime = _process.TotalProcessorTime;
            }
        }

        /// <summary>
        /// Refresh the process
        /// </summary>
        public void Refresh()
        {
            if (UserName != "SYSTEM")
            {
                _watch.Stop();

                var newTotalProcessorTime = _process.TotalProcessorTime;
                var cpuUsedMilliseconds = (newTotalProcessorTime - _totalProcessorTime).TotalMilliseconds;

                var cpuUsageTotal = cpuUsedMilliseconds / (Environment.ProcessorCount * _watch.ElapsedMilliseconds);

                CpuUsage = Math.Round(cpuUsageTotal * 100, 2);
                _totalProcessorTime = newTotalProcessorTime;

                _watch.Restart();
            }
            MemoryUsageMB = _process.WorkingSet64 / 1024 / 1024;
        }
    }
}