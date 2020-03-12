namespace wttop.Core
{
    /// <summary>
    /// Process information
    /// </summary>
    public class ProcessInfo
    {
        /// <summary>
        /// Process name
        /// </summary>
        public string Name {get; set; }

        /// <summary>
        /// Percentage of processor time
        /// </summary>
        public double PercentProcessorTime {get; set; }

        /// <summary>
        /// Process ID
        /// </summary>
        public int IdProcess {get; set; }

        /// <summary>
        /// Process owner
        /// </summary>
        public string Owner {get; set;}

        /// <summary>
        /// Number of thread
        /// </summary>
        public int ThreadCount {get; set; }

        /// <summary>
        /// Memory usage in mega bytes
        /// </summary>
        public long MemoryUsageMb {get; set; }
    }
}