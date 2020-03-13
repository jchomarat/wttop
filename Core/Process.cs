using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core
{
    /// <summary>
    /// The process class
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Get or set the list of processes
        /// </summary>
        public List<ProcessInfo> Processes { get; set; } = new List<ProcessInfo>();

        /// <summary>
        /// Get the top 15 processes by CPU usage
        /// </summary>
        public IEnumerable<ProcessInfo> GetTop15CPU => Processes
                    .OrderByDescending(p => p.PercentProcessorTime)
                    .Take(15);

        /// <summary>
        /// Get the top 15 processes by memory usage
        /// </summary>
        public IEnumerable<ProcessInfo> GetTop15Memory => Processes
                    .OrderByDescending(p => p.MemoryUsageMb)
                    .Take(15);
        /// <summary>
        /// Get the number of processes running
        /// </summary>
        public int ProcessesCount => Processes.Count();
    }
}