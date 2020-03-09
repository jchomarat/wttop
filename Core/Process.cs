using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core {

    public class ProcessInfo
    {
        public string Name {get; set; }

        public double PercentProcessorTime {get; set; }

        public int IDProcess {get; set; }

        public string Owner {get; set;}

        public int ThreadCount {get; set; }

        public long MemoryUsageMb {get; set; }
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

        public IEnumerable<ProcessInfo> GetTop15CPU
        {
            get
            {
                return processes
                    .OrderByDescending(p => p.PercentProcessorTime)
                    .Take(15);
            }
        }

        public IEnumerable<ProcessInfo> GetTop15Memory
        {
            get
            {
                return processes
                    .OrderByDescending(p => p.MemoryUsageMb)
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