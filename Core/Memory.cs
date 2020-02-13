using System;

namespace wttop.Core {

    public class Memory {
        
        public int TotalKb {get; set; }
       
        public int AvailableKb {get; set; }

        public int TotalSwapKb {get; set; }
       
        public int AvailableSwapKb {get; set; }

        public decimal AvailableGB {
            get {
                return Math.Round((decimal)AvailableKb/(1024*1024), 1); 
            }
        }

        public decimal AvailableSwapGB {
            get {
                return Math.Round((decimal)AvailableSwapKb/(1024*1024), 1); 
            }
        }

        public int UsedKb {
            get {
                return TotalKb - AvailableKb;
            }
        }

        public int UsedSwapKb {
            get {
                return TotalSwapKb - AvailableSwapKb;
            }
        }

        public decimal UsedGB {
            get {
                return Math.Round((decimal)UsedKb/(1024*1024), 1); 
            }
        }

        public decimal UsedSwapGB {
            get {
                return Math.Round((decimal)UsedSwapKb/(1024*1024), 1); 
            }
        }

        public decimal TotalGB {
            get {
                return Math.Round((decimal)TotalKb/(1024*1024), 1); 
            }
        }

        public decimal TotalSwapGB {
            get {
                return Math.Round((decimal)TotalSwapKb/(1024*1024), 1); 
            }
        }

        public int PercentageUsed {
            get {
                return (int)Math.Floor((decimal)(100*UsedKb)/TotalKb);
            }
        }

        public int PercentageSwapUsed {
            get {
                return (int)Math.Floor((decimal)(100*UsedSwapKb)/TotalSwapKb);
            }
        }

        public int PercentageAvailable {
            get {
                return (int)Math.Floor((decimal)(100*AvailableKb)/TotalKb);
            }
        }

        public int PercentageSwapAvailable {
            get {
                return (int)Math.Floor((decimal)(100*AvailableSwapKb)/TotalSwapKb);
            }
        }
    }
}