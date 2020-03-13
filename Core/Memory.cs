using System;

namespace wttop.Core {

    /// <summary>
    /// Memory class
    /// </summary>
    public class Memory {
        
        /// <summary>
        /// Total physical size of the memory in giga bytes
        /// </summary>
        public int PhysicalTotalGb {get; set; }

        /// <summary>
        /// Percentage of used physical memory
        /// </summary>
        public int PhysicalPercentageUsed {get; set;}

        /// <summary>
        /// Physical available memory in giga bytes
        /// </summary>
        public int PhysicalAvailableGb {get; set; }

        /// <summary>
        /// Total size of the swap file in giga bytes
        /// </summary>
        public int SwapTotalGb {get; set; }

        /// <summary>
        /// Swap percentage used
        /// </summary>
        public int SwapPercentageUsed {get; set;}

        /// <summary>
        /// Swap available memory in giga bytes
        /// </summary>
        public int SwapAvailableGb {get; set; }
    }
}