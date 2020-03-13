using System.Collections.Generic;

namespace wttop.Core
{
    /// <summary>
    /// Class to compare the processes
    /// </summary>
    internal class ProcessEqualityComparer : IEqualityComparer<ProcessWrapper>
    {
        /// <summary>
        /// Are the processes equal
        /// </summary>
        /// <param name="pd1">First process wrapper to compare</param>
        /// <param name="pd2">Second process wrapper to compare</param>
        /// <returns>True if equal</returns>
        public bool Equals(ProcessWrapper pd1, ProcessWrapper pd2) => pd1.Pid == pd2.Pid;

        /// <summary>
        /// Get the process hash code
        /// </summary>
        /// <param name="pd">Process wrapper</param>
        /// <returns>The hash code</returns>
        public int GetHashCode(ProcessWrapper pd)
        {
            string instance = pd.ToString();
            return instance.GetHashCode();
        }
    }
}