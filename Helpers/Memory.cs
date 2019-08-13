using System;

namespace wttop.Helpers {

    public class Memory {
        
        public int TotalKb {get; set;}
       
        public int AvailableKb {get;set;}

        public int UsedKb {
            get {
                return TotalKb - AvailableKb;
            }
        }

        public int UsedGB {
            get {
                return (int)Math.Floor((decimal)TotalKb/(1024*1024));
            }
        }

        public int TotalGB {
            get {
                return (int)Math.Floor((decimal)TotalKb/(1024*1024));
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