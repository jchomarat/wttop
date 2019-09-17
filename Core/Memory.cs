using System;

namespace wttop.Core {

    public class Memory {
        
        public int TotalKb {get; set; }
       
        public int AvailableKb {get; set; }

        public decimal AvailableGB {
            get {
                return Math.Round((decimal)AvailableKb/(1024*1024), 1); 
            }
        }

        public int UsedKb {
            get {
                return TotalKb - AvailableKb;
            }
        }

        public decimal UsedGB {
            get {
                return Math.Round((decimal)UsedKb/(1024*1024), 1); 
            }
        }

        public decimal TotalGB {
            get {
                return Math.Round((decimal)TotalKb/(1024*1024), 1); 
            }
        }

        public int PercentageUsed {
            get {
                return (int)Math.Floor((decimal)(100*UsedKb)/TotalKb);
            }
        }

        public int PercentageAvailable {
            get {
                return (int)Math.Floor((decimal)(100*AvailableKb)/TotalKb);
            }
        }
    }
}