using System;

namespace wttop.Core {

    public class Memory {
        
        public int PhysicalTotalGb {get; set; }

        public int PhysicalPercentageUsed {get; set;}

        public int PhysicalAvailableGb {get; set; }

        public int SwapTotalGb {get; set; }

        public int SwapPercentageUsed {get; set;}

        public int SwapAvailableGb {get; set; }
    }
}