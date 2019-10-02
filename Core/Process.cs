using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core {

    public class ProcessInfo
    {
        public string Name {get; set; }

        public int PercentProcessorTime {get; set; }

        public int IDProcess {get; set; }

        public int ThreadCount {get; set; }

        public int HandleCount {get; set; }

        public long MemoryUsageB {get; set; }

        public double MemoryUsageMB
        {
            get
            {
                double d = MemoryUsageB/1024.0/1024.0;
                return Math.Round(d, 1);
            }
        }

        public int PriorityBase {get; set; }
    }

    public class Process
    {
        List<ProcessInfo> processes = new List<ProcessInfo>();

        public List<ProcessInfo> Processes
        {
            get
            {
                return processes;
            }
            set 
            {
                processes = value;
            }
        }

        public IEnumerable<ProcessInfo> GetTop10
        {
            get
            {
                return processes
                    .OrderByDescending(p => p.PercentProcessorTime)
                    .Take(10);
            }
        }

        public IEnumerable<ProcessInfo> GetTop15
        {
            get
            {
                return processes
                    .OrderByDescending(p => p.PercentProcessorTime)
                    .Take(15);
            }
        }
        
        public int ProcessesCount
        {
            get 
            {
                return processes.Count();
            }
        }
    }
}