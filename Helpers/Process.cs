using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class ProcessInfo
    {
        public string Name {get; set; }

        public int PercentProcessorTime {get; set; }

        public int IDProcess {get; set; }

        public int ThreadCount {get; set; }

        public int HandleCOunt {get; set; }

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
        
        public int ProcessesCount
        {
            get 
            {
                return processes.Count();
            }
        }
    }
}