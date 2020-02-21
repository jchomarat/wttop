using System;
using System.Collections.Generic;
using SD = System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    internal class ProcessEqualityComparer : IEqualityComparer<ProcessWrapper>
    {
        public bool Equals(ProcessWrapper pd1, ProcessWrapper pd2)
        {
            return (pd1.Pid == pd2.Pid);
        }

        public int GetHashCode(ProcessWrapper pd)
        {
            string instance = pd.ToString();
            return instance.GetHashCode();
        }
    }

    internal class ProcessWrapper
    {
        SD.Process process;

        TimeSpan totalProcessorTime;

        SD.Stopwatch watch;

        public ProcessWrapper(SD.Process Process)
        {
            process = Process;
            parseProcess();
            onCreated();
        }

        public int Pid {get; set;}

        public string Name {get; set;}

        public int ThreadCount {get;set;}

        public string FileName {get; set;}

        public string UserName {get; set;}

        public long MemoryUsageMB {get;set;}

        public double CpuUsage {get; set;} = 0;

        public override string ToString(){
            return $"{CpuUsage} %, {Name} {MemoryUsageMB} Mb (owner: {UserName})";
        }

        void parseProcess()
        {
            Name = process.ProcessName;
            Pid = process.Id;
            ThreadCount = process.Threads.Count;
            MemoryUsageMB = process.WorkingSet64/1024/1024;

            try 
            {
                FileName = process.MainModule.FileName;
                UserName = retrieveUserName();
            }
            catch
            {
                FileName = "";
                UserName = "SYSTEM";
            }
        }

        string retrieveUserName()
        {
            IntPtr buffer;
            int strLen;
            string username = string.Empty;
            if (WtsApi32.WTSQuerySessionInformation(IntPtr.Zero, process.SessionId, WtsApi32.WtsInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer);
                WtsApi32.WTSFreeMemory(buffer);
            }
            return username;
        }

        void onCreated()
        {
            // Does not work for system process with managed code
            if (UserName != "SYSTEM")
            {
                watch = new SD.Stopwatch();
                watch.Start();
                totalProcessorTime = process.TotalProcessorTime;
            }
        }

        public void Refresh()
        {
            if (UserName != "SYSTEM")
            {
                watch.Stop();
                
                var newTotalProcessorTime = process.TotalProcessorTime;
                var cpuUsedMilliseconds = (newTotalProcessorTime - totalProcessorTime).TotalMilliseconds;

                var cpuUsageTotal = cpuUsedMilliseconds / (Environment.ProcessorCount * watch.ElapsedMilliseconds);

                CpuUsage = Math.Round(cpuUsageTotal*100, 2);
                totalProcessorTime = newTotalProcessorTime;

                watch.Restart();
            }
            MemoryUsageMB = process.WorkingSet64/1024/1024;
        }
    }

    internal class ProcessesReader
    {
        List<ProcessWrapper> processWrapper;

        public ProcessWrapper[] ProcessWrapper
        {
            get { return processWrapper.ToArray(); }
        }

        public ProcessesReader()
        {
            FillProcessList();
        }

        void FillProcessList()
        {
            SD.Process[] processlist = SD.Process.GetProcesses();

            // First call
            processWrapper = processlist.Select(p => {
                return new ProcessWrapper(p);
            }).ToList();
        }

        public ProcessWrapper[] Refresh()
        {
            SD.Process[] processlist = SD.Process.GetProcesses();

            // Remove dead processes
            var tmpList = processWrapper.Where(pd => processlist.Any(p => p.Id == pd.Pid)).ToList();

            // Add new ones
            var newProcess = processlist
                                .Where(p => !processWrapper.Any(pd => pd.Pid == p.Id))
                                .Select(p => { return new ProcessWrapper(p); } ).ToList();
            
            tmpList.AddRange(newProcess);

            processWrapper = tmpList;

            // Refresh metric
            processWrapper.ToList().ForEach(p => p.Refresh());

            return processWrapper.ToArray();
        }
    }
}